using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Dto.Prison;
using JailTalk.Shared;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.ReleasedPrisoner;

public class ReleasedPrisonerLastAssociatedJailDetailQuery : IRequest<PrisonerLastAssociatedJailDetailDto>
{
    public Guid PrisonerId { get; set; }
}
public class ReleasedPrisonerLastAssociatedJailDetailQueryHandler : IRequestHandler<ReleasedPrisonerLastAssociatedJailDetailQuery, PrisonerLastAssociatedJailDetailDto>
{
    readonly IAppDbContext _dbContext;

    public ReleasedPrisonerLastAssociatedJailDetailQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PrisonerLastAssociatedJailDetailDto> Handle(ReleasedPrisonerLastAssociatedJailDetailQuery request, CancellationToken cancellationToken)
    {
        var prisoner = await _dbContext.PrisonerFunctions.Where(x => x.PrisonerId == request.PrisonerId)
            .Select(x => new
            {
                x.LastAssociatedJail.Code,
                x.LastAssociatedJail.Name,
                x.LastReleasedOn,
                CurrentJailId = x.Prisoner.JailId
            })
            .SingleOrDefaultAsync(cancellationToken) ?? throw new AppException(CommonExceptionMessages.PrisonerNotFound);

        return new PrisonerLastAssociatedJailDetailDto
        {
            LastAssociatedPrison = $"({prisoner.Code}) {prisoner.Name}",
            LastReleasedOn = prisoner.LastReleasedOn.Value.ToLocalDateTimeString(),
            IsPrisonerAssociatedToJail = prisoner.CurrentJailId.HasValue
        };
    }
}

