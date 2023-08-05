using Humanizer;
using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Dto.Prison;
using JailTalk.Shared;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prison;

public class CallHistoryQuery : IRequest<List<CallHistoryListDto>>
{
    public Guid PrisonerId { get; set; }
    public int LastNDays { get; set; }
}
public class CallHistoryQueryHandler : IRequestHandler<CallHistoryQuery, List<CallHistoryListDto>>
{
    readonly IAppDbContext _dbContext;

    public CallHistoryQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<CallHistoryListDto>> Handle(CallHistoryQuery request, CancellationToken cancellationToken)
    {
        var callHistory = await _dbContext.CallHistory
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
                RelativeName = x.PhoneDirectory.RelativeType.Value
            })
            .ToListAsync(cancellationToken);
        int index = 1;
        return callHistory.Select(x => new CallHistoryListDto
        {
            Id = x.Id,
            Serial = index++,
            CallDuration = x.EndedOn.HasValue ? ((float?)(x.EndedOn.Value - x.CallStartedOn).TotalMinutes).ToHoursMinutesSeconds() : AppStringConstants.GridNoDataIndication,
            CallStartedOn = x.CallStartedOn.ToLocalTime().ToString(),
            CallEndedOn = x.EndedOn.HasValue ? x.EndedOn.Value.Humanize() : AppStringConstants.GridNoDataIndication,
            ContactNumber = $"{x.CountryCode} {x.PhoneNumber}",
            Callee = x.RelativeName,
        }).ToList();
    }
}


