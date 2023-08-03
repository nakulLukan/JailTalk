using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Prison;
using JailTalk.Shared;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace JailTalk.Application.Requests.Prison;

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
        Guid prisonerId = await ValidateAndGetPrisonerId(request);

        var phoneBalanceEntity = await _dbContext.PhoneBalances
            .Where(x => x.PrisonerId == prisonerId)
            .Select(x => new
            {
                x.Id,
                x.Balance,
                PrisonerGender = x.Prisoner.Gender
            })
            .FirstOrDefaultAsync(cancellationToken);

        int availableTalkTime = await ValidateAndGetAvailableTalkTime(
            prisonerId,
            phoneBalanceEntity.PrisonerGender,
            phoneBalanceEntity.Balance,
            cancellationToken);
        RequestCallResultDto response = await AddCallHistoryEntryGetResponse(request,
                                                                             availableTalkTime,
                                                                             cancellationToken);

        _logger.LogInformation("Call Allowed for prisoner: {prisoner}, Contact Id: {ct}, Duration: {dur}", prisonerId, request.ContactId, response.AvailableTalkTime / 60);
        return response;
    }

    private async Task<Guid> ValidateAndGetPrisonerId(RequestCallCommand request)
    {
        var prisonerId = _requestContext.GetPrisonerId();
        var contact = await _dbContext.PhoneDirectory
            .Where(x => x.PrisonerId == prisonerId
                && x.Id == request.ContactId)
            .Select(x => new
            {
                x.IsActive,
                x.IsBlocked
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

        return prisonerId;
    }

    private async Task<RequestCallResultDto> AddCallHistoryEntryGetResponse(RequestCallCommand request, int availableTalkTime, CancellationToken cancellationToken)
    {
        var callHistory = new Domain.Prison.CallHistory
        {
            PhoneDirectoryId = request.ContactId,
            CallStartedOn = AppDateTime.UtcNow,
            EndedOn = null,
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

    private async Task<int> ValidateAndGetAvailableTalkTime(Guid prisonerId, Gender prisonerGender, float phoneBalance, CancellationToken cancellationToken)
    {
        // Get amount charged for a phone call per minute.
        var chargePerMinute = await _appSettingsProvider.GetCallPriceChargedPerMinute();

        // Call duration per day varies for each gender.
        var maxAllowedTalkTimeInMinutes = await _appSettingsProvider.GetMaxAllowedCallDuration(prisonerGender);

        var todaysTotalTalkTime = await _dbContext.CallHistory
            .Where(x => x.PhoneDirectory.PrisonerId == prisonerId
                && x.EndedOn.HasValue && x.CallStartedOn >= AppDateTime.UtcNowAtStartOfTheDay)
            .SumAsync(x => (x.EndedOn.Value - x.CallStartedOn).TotalSeconds, cancellationToken);


        // Available talk time = Minimum of allowed talk time in seconds VS available balance amount converted to value per second - Total talktime per day.
        var availableTalkTime = Convert.ToInt32(
                Math.Ceiling(MathF.Min(maxAllowedTalkTimeInMinutes * 60,
                MathF.Ceiling((phoneBalance / chargePerMinute) * 60))));
        availableTalkTime = (int)Math.Max(0, availableTalkTime - todaysTotalTalkTime);
        if (availableTalkTime < MINIMUM_TALK_TIME_REQUIRED)
        {
            throw new AppApiException(HttpStatusCode.UnprocessableEntity, "RC-0003", "Balance talktime is too low to make a call.");
        }

        return availableTalkTime;
    }
}
