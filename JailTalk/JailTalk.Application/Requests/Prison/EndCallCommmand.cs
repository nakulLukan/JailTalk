﻿using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Prison;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace JailTalk.Application.Requests.Prison;

public class EndCallCommmand : IRequest<EndCallResultDto>
{
    public int CallHistoryId { get; set; }
}

public class EndCallCommmandHandler : IRequestHandler<EndCallCommmand, EndCallResultDto>
{
    readonly IAppDbContext _appDbContext;
    readonly ILogger<EndCallCommmandHandler> _logger;
    readonly IDeviceRequestContext _requestContext;


    public EndCallCommmandHandler(IAppDbContext appDbContext, IDeviceRequestContext requestContext, ILogger<EndCallCommmandHandler> logger)
    {
        _appDbContext = appDbContext;
        _requestContext = requestContext;
        _logger = logger;
    }

    public async Task<EndCallResultDto> Handle(EndCallCommmand request, CancellationToken cancellationToken)
    {
        var prisonerId = _requestContext.GetPrisonerId();

        var callHistory = await _appDbContext.CallHistory.AsTracking()
            .Include(x => x.PhoneDirectory)
            .Where(x => x.Id == request.CallHistoryId)
            .FirstOrDefaultAsync(cancellationToken);

        if (callHistory == null)
        {
            throw new AppApiException(HttpStatusCode.NotFound, "Invalid call history record");
        }

        if (prisonerId != callHistory.PhoneDirectory.PrisonerId)
        {
            _logger.LogError("Record does not belong to prisoner {prisoner}", prisonerId);
            throw new AppApiException(HttpStatusCode.BadRequest, "Invalid call history record");
        }

        if (callHistory.EndedOn.HasValue)
        {
            _logger.LogError("Call already terminated on {terminatedOn}", callHistory.EndedOn.Value);
            throw new AppApiException(HttpStatusCode.BadRequest, "Call Already Terminated");
        }

        var phoneBalance = await _appDbContext.PhoneBalances.AsTracking()
            .Where(x => x.PrisonerId == prisonerId)
            .FirstOrDefaultAsync(cancellationToken);

        callHistory.EndedOn = AppDateTime.UtcNow;
        callHistory.CallTerminationReason = Shared.CallEndReason.CallEnded;
        var totalCallDuration = callHistory.EndedOn.Value - callHistory.CallStartedOn;
        var callCost = (float)totalCallDuration.TotalMinutes;

        phoneBalance.Balance -= callCost;
        phoneBalance.LastUpdatedOn = AppDateTime.UtcNow;

        _appDbContext.PhoneBalanceHistory.Add(new()
        {
            AmountDifference = callCost,
            CallRequestId = request.CallHistoryId,
            CreatedOn = AppDateTime.UtcNow,
            NetAmount = phoneBalance.Balance,
            PrisonerId = prisonerId,
            Reason = Shared.PhoneBalanceReason.RegularCall,
            RechargedByUserId = null,
        });
        await _appDbContext.SaveAsync(cancellationToken);

        return new EndCallResultDto
        {
            AvailableBalance = phoneBalance.Balance,
            CallDuration = $"{totalCallDuration.Minutes} minutes {totalCallDuration.Seconds} seconds"
        };
    }
}

