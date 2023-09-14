using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Prison;
using JailTalk.Application.Services;
using JailTalk.Shared;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prison;

public class GetPrisonerSecondaryDetailsQuery : IRequest<PrisonerSecondaryDetailDto>
{
    public Guid PrisonerId { get; set; }
}
public class GetPrisonerSecondaryDetailsQueryHandler : IRequestHandler<GetPrisonerSecondaryDetailsQuery, PrisonerSecondaryDetailDto>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;

    public GetPrisonerSecondaryDetailsQueryHandler(IAppDbContext dbContext, IAppRequestContext requestContext)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
    }

    public async Task<PrisonerSecondaryDetailDto> Handle(GetPrisonerSecondaryDetailsQuery request, CancellationToken cancellationToken)
    {
        var prisonId = _requestContext.GetAssociatedPrisonId();
        var secondaryDetails = await _dbContext.PrisonerFunctions
            .Where(x => x.PrisonerId == request.PrisonerId)
            .WhereInPrison(x => x.Prisoner.JailId, prisonId)
            .Select(x => new
            {
                x.Prisoner.Pid,
                x.PrisonerId,
                x.Prisoner.IsBlocked,
                x.PunishmentEndsOn,
                x.UnlimitedCallsEndsOn,
                x.LastReleasedOn,
                x.Prisoner.JailId,
                LastAssociatedJailCode = x.LastAssociatedJail.Code,
            })
            .FirstOrDefaultAsync(cancellationToken) ?? throw new AppException(CommonExceptionMessages.PrisonerNotFound, true);
        return new PrisonerSecondaryDetailDto
        {
            RetrictionEndsOn = secondaryDetails.PunishmentEndsOn,
            PrisonerId = request.PrisonerId,
            IsBlocked = PrisonerHelper.IsPrisonerBlocked(secondaryDetails.IsBlocked, secondaryDetails.PunishmentEndsOn),
            LastReleasedOn = secondaryDetails.LastReleasedOn,
            IsUnlimitedCallEnabled = PrisonerHelper.IsUnlimitedCallPriviledgeEnabled(secondaryDetails.UnlimitedCallsEndsOn),
            IsReleased = !secondaryDetails.JailId.HasValue,
            LastAssociatedJailCode = secondaryDetails.LastAssociatedJailCode,
            Pid = secondaryDetails.Pid
        };
    }

}
