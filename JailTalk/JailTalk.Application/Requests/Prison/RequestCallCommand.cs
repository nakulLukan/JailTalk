using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Prison;
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

    public RequestCallCommandHandler(IAppDbContext dbContext, IDeviceRequestContext requestContext, ILogger<RequestCallCommandHandler> logger, IApplicationSettingsProvider appSettingsProvider)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
        _logger = logger;
        _appSettingsProvider = appSettingsProvider;
    }

    public async Task<RequestCallResultDto> Handle(RequestCallCommand request, CancellationToken cancellationToken)
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
            throw new AppApiException(HttpStatusCode.BadRequest, "Invalid Contact Requested");
        }
        else if (contact.IsBlocked || !contact.IsActive)
        {
            _logger.LogError("Contact '{ct}' active status: {active}, is blocked: {blocked}", request.ContactId, contact.IsActive, contact.IsBlocked);
            throw new AppApiException(HttpStatusCode.BadRequest, "Invalid Contact Requested");
        }

        var phoneBalanceEntity = await _dbContext.PhoneBalances
            .Where(x => x.PrisonerId == prisonerId)
            .Select(x => new
            {
                x.Id,
                x.Balance,
                PrisonerGender = x.Prisoner.Gender
            })
            .FirstOrDefaultAsync(cancellationToken);
        var callHistory = new Domain.Prison.CallHistory
        {
            PhoneDirectoryId = request.ContactId,
            CallStartedOn = AppDateTime.UtcNow,
            EndedOn = null,
        };
        _dbContext.CallHistory.Add(callHistory);
        await _dbContext.SaveAsync(cancellationToken);

        var maxAllowedTalkTimeInMinutes = await _appSettingsProvider.GetMaxAllowedCallDuration(phoneBalanceEntity.PrisonerGender);
        var response = new RequestCallResultDto
        {
            CallHistoryId = callHistory.Id,
            AvailableTalkTime = Convert.ToInt32(Math.Ceiling(MathF.Min(maxAllowedTalkTimeInMinutes * 60, phoneBalanceEntity.Balance * 60))),
        };

        _logger.LogInformation("Call Allowed for prisoner: {prisoner}, Contact Id: {ct}, Duration: {dur}", prisonerId, request.ContactId, response.AvailableTalkTime / 60);
        return response;
    }
}
