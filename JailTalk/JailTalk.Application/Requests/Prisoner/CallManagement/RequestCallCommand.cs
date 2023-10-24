using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Prison;
using JailTalk.Application.Services;
using JailTalk.Shared;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace JailTalk.Application.Requests.Prisoner.CallManagement;

public class RequestCallCommand : IRequest<RequestCallResultDto>
{
    public int ContactId { get; set; }
}

public class RequestCallCommandHandler : IRequestHandler<RequestCallCommand, RequestCallResultDto>
{
    readonly IAppDbContext _dbContext;
    readonly IDeviceRequestContext _requestContext;
    readonly IApplicationSettingsProvider _appSettingsProvider;
    readonly ILogger<RequestCallCommandHandler> _logger;

    static readonly CallEndReason[] ActiveCallStatus = new CallEndReason[]
    {
        CallEndReason.CallConference,
        CallEndReason.InsufficientBalance,
        CallEndReason.CallTimeExpired,
        CallEndReason.CallerRegularCut,
        CallEndReason.RecieverRegularCut,
    };

    /// <summary>
    /// Minimum number of seconds required in balance talktime to begin a phone call  
    /// </summary>
    const int MINIMUM_TALK_TIME_REQUIRED = 15;

    public RequestCallCommandHandler(IAppDbContext dbContext, IDeviceRequestContext requestContext, ILogger<RequestCallCommandHandler> logger, IApplicationSettingsProvider appSettingsProvider)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
        _logger = logger;
        _appSettingsProvider = appSettingsProvider;
    }

    public async Task<RequestCallResultDto> Handle(RequestCallCommand request, CancellationToken cancellationToken)
    {
        (Guid prisonerId, int? prisonerJailId) = await ValidateAndGetPrisonerId(request);

        var phoneBalanceEntity = await _dbContext.PhoneBalances
            .Where(x => x.PrisonerId == prisonerId)
            .Select(x => new
            {
                x.Id,
                x.Balance,
                PrisonerGender = x.Prisoner.Gender,
                IsUnlimitedCallPriviledgeEnabled = PrisonerHelper.IsUnlimitedCallPriviledgeEnabled(x.Prisoner.PrisonerFunction.UnlimitedCallsEndsOn)
            })
            .FirstOrDefaultAsync(cancellationToken);

        int availableTalkTime = await ValidateAndGetAvailableTalkTime(
            prisonerId,
            phoneBalanceEntity.PrisonerGender,
            phoneBalanceEntity.Balance,
            phoneBalanceEntity.IsUnlimitedCallPriviledgeEnabled,
            cancellationToken);
        RequestCallResultDto response = await AddCallHistoryEntryGetResponse(request,
                                                                             availableTalkTime,
                                                                             prisonerJailId,
                                                                             cancellationToken);

        _logger.LogInformation("Call Allowed for prisoner: {prisoner}, Contact Id: {ct}, Duration: {dur}", prisonerId, request.ContactId, response.AvailableTalkTime / 60);
        return response;
    }

    private async Task<(Guid PrisonerId, int? jailId)> ValidateAndGetPrisonerId(RequestCallCommand request)
    {
        var prisonerId = _requestContext.GetPrisonerId();
        var contact = await _dbContext.PhoneDirectory
            .Where(x => x.PrisonerId == prisonerId
                && x.Id == request.ContactId)
            .Select(x => new
            {
                x.IsActive,
                x.IsBlocked,
                x.Prisoner.JailId
            })
            .FirstOrDefaultAsync();
        if (contact is null)
        {
            _logger.LogError("Contact '{ct}' is associated to prisoner {prisoner}", request.ContactId, prisonerId);
            throw new AppApiException(HttpStatusCode.BadRequest, "RC-0001", "Invalid Contact Requested");
        }
        else if (contact.IsBlocked || !contact.IsActive)
        {
            _logger.LogError("Contact '{ct}' active status: {active}, is blocked: {blocked}", request.ContactId, contact.IsActive, contact.IsBlocked);
            throw new AppApiException(HttpStatusCode.BadRequest, "RC-0002", "Invalid Contact Requested");
        }

        if (!contact.JailId.HasValue)
        {
            throw new AppApiException(HttpStatusCode.BadRequest, "RC-0004", "Prisoner is not associated to any prison");
        }
        return (prisonerId, contact.JailId);
    }

    private async Task<RequestCallResultDto> AddCallHistoryEntryGetResponse(RequestCallCommand request, int availableTalkTime, int? prisonerJailId, CancellationToken cancellationToken)
    {
        var callHistory = new Domain.Prison.CallHistory
        {
            PhoneDirectoryId = request.ContactId,
            CallStartedOn = AppDateTime.UtcNow,
            EndedOn = null,
            AssociatedPrisonId = prisonerJailId.Value
        };
        _dbContext.CallHistory.Add(callHistory);
        await _dbContext.SaveAsync(cancellationToken);
        var response = new RequestCallResultDto
        {
            CallHistoryId = callHistory.Id,
            AvailableTalkTime = availableTalkTime,
        };
        return response;
    }

    private async Task<int> ValidateAndGetAvailableTalkTime(
        Guid prisonerId,
        Gender prisonerGender,
        float phoneBalance,
        bool isUnlimitedCallPriviledgeEnabled,
        CancellationToken cancellationToken)
    {
        // Get amount charged for a phone call per minute.
        var chargePerMinute = await _appSettingsProvider.GetCallPriceChargedPerMinute();

        // Call duration per day varies for each gender.
        var maxAllowedTalkTimeInMinutes = await _appSettingsProvider.GetMaxAllowedCallDuration(prisonerGender);

        // If unlimited call is enabled then do not consider todays talk time.
        var todaysTotalTalkTime = !isUnlimitedCallPriviledgeEnabled ? await _dbContext.CallHistory
            .Where(x => x.PhoneDirectory.PrisonerId == prisonerId
                && ActiveCallStatus.Contains(x.CallTerminationReason)
                && x.EndedOn.HasValue
                && x.CallStartedOn >= AppDateTime.UtcNowAtStartOfTheDay)
            .SumAsync(x => (x.EndedOn.Value - x.CallStartedOn).TotalSeconds, cancellationToken)
        : 0;


        // Available talk time = Minimum of allowed talk time in seconds VS available balance amount converted to value per second - Total talktime per day.
        // If the user is allowed to have unlimited calls till end of the day then allow the user to consume all his balance
        float secondsAllowedIfCallPriviledgeEnabled = isUnlimitedCallPriviledgeEnabled ? (float)(AppDateTime.UtcNow - AppDateTime.TillEndOfDay).TotalSeconds : 0F;
        var availableCallTimeInSeconds = MathF.Max(maxAllowedTalkTimeInMinutes * 60, secondsAllowedIfCallPriviledgeEnabled);
        var availableTalkTime = (int)Math.Min(availableCallTimeInSeconds,
                                                MathF.Ceiling(phoneBalance / chargePerMinute * 60));
        availableTalkTime = (int)Math.Max(0, availableTalkTime - todaysTotalTalkTime);
        if (availableTalkTime < MINIMUM_TALK_TIME_REQUIRED)
        {
            throw new AppApiException(HttpStatusCode.UnprocessableEntity, "RC-0003", "Balance talktime is too low to make a call.");
        }

        return availableTalkTime;
    }
}
