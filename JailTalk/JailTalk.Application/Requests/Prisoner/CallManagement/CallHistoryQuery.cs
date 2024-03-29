﻿using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Prison;
using JailTalk.Shared;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prisoner.CallManagement;

public class CallHistoryQuery : IRequest<List<CallHistoryListDto>>
{
    public Guid PrisonerId { get; set; }
    public int LastNDays { get; set; }
}
public class CallHistoryQueryHandler : IRequestHandler<CallHistoryQuery, List<CallHistoryListDto>>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;
    readonly IApplicationSettingsProvider _settingsProvider;

    public CallHistoryQueryHandler(IAppDbContext dbContext,
                                   IAppRequestContext requestContext,
                                   IApplicationSettingsProvider settingsProvider)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
        _settingsProvider = settingsProvider;
    }

    public async Task<List<CallHistoryListDto>> Handle(CallHistoryQuery request, CancellationToken cancellationToken)
    {
        var callHistory = await _dbContext.CallHistory
            .WhereInPrison(x => x.AssociatedPrisonId, _requestContext.GetAssociatedPrisonId())
            .Where(x => x.PhoneDirectory.PrisonerId == request.PrisonerId)
            .Where(x => x.CallStartedOn >= AppDateTime.UtcDateBeforeNDays(request.LastNDays))
            .OrderByDescending(x => x.CallStartedOn)
            .Select(x => new
            {
                x.Id,
                x.CallStartedOn,
                x.EndedOn,
                x.PhoneDirectory.PhoneNumber,
                x.PhoneDirectory.CountryCode,
                RelativeName = $"{x.PhoneDirectory.Name} ({x.PhoneDirectory.RelativeType.Value})",
                x.CallRecordingAttachment.FileName,
                x.CallTerminationReason
            })
            .ToListAsync(cancellationToken);
        int index = 1;
        bool accessToCallRecordingAllowed = await _settingsProvider.GetAllowAccessToCallRecording();
        return callHistory.Select(x => new CallHistoryListDto
        {
            Id = x.Id,
            Serial = index++,
            CallDuration = x.EndedOn.HasValue ? ((float?)(x.EndedOn.Value - x.CallStartedOn).TotalMinutes).ToHoursMinutesSeconds() : AppStringConstants.GridNoDataIndication,
            CallStartedOn = x.CallStartedOn.ToLocalDateTimeString(),
            CallEndedOn = x.EndedOn.HasValue ? x.EndedOn.Value.ToLocalDateTimeString() : AppStringConstants.GridNoDataIndication,
            ContactNumber = $"{x.CountryCode} {x.PhoneNumber}",
            Callee = x.RelativeName,
            CallEndReason = GetCallEndReasonAsText(x.CallTerminationReason),
            CallRecordingState = accessToCallRecordingAllowed ? !string.IsNullOrEmpty(x.FileName) : null
        }).ToList();
    }

    private string GetCallEndReasonAsText(CallEndReason value)
    {
        return value switch
        {
            CallEndReason.CallTimeExpired => "Maximum allowed time reached",
            CallEndReason.CallerSkippedCall => "Call Skipped",
            CallEndReason.CallConference => "Call Conference",
            CallEndReason.MissedCall => "Reciever Missed Call",
            CallEndReason.CallerRegularCut or CallEndReason.RecieverRegularCut => "Regular Call",
            CallEndReason.InsufficientBalance => "Insufficient Balance",
            CallEndReason.NetworkError => "Network Error",
            CallEndReason.RecieverBusy => "Busy",
            _ => "-",
        };
    }
}


