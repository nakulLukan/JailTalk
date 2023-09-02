using JailTalk.Application.Contracts.Data;
using JailTalk.Shared;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.ReleasedPrisoner;

public class ReleasedPrisonerLastAssociatedJailDetailQuery : IRequest<(string LastAssociatedPrison, string LastReleasedOn)>
{
    public Guid PrisonerId { get; set; }
}
public class ReleasedPrisonerLastAssociatedJailDetailQueryHandler : IRequestHandler<ReleasedPrisonerLastAssociatedJailDetailQuery, (string LastAssociatedPrison, string LastReleasedOn)>
{
    readonly IAppDbContext _dbContext;

    public ReleasedPrisonerLastAssociatedJailDetailQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<(string LastAssociatedPrison, string LastReleasedOn)> Handle(ReleasedPrisonerLastAssociatedJailDetailQuery request, CancellationToken cancellationToken)
    {
        var prisoner = await _dbContext.PrisonerFunctions.Where(x => x.PrisonerId == request.PrisonerId)
            .Select(x => new
            {
                x.LastAssociatedJail.Code,
                x.LastAssociatedJail.Name,
                x.LastReleasedOn
            })
            .SingleOrDefaultAsync(cancellationToken) ?? throw new AppException(CommonExceptionMessages.PrisonerNotFound);

        return ($"({prisoner.Code}) {prisoner.Name}", prisoner.LastReleasedOn.Value.ToLocalDateTimeString());
    }
}

