using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Shared;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prison;

public class ReleasePrisonerFromJailCommand : IRequest<bool>
{
    public Guid PrisonerId { get; set; }

    /// <summary>
    /// If value is true then the prisoner will be released from the assigned prison.
    /// </summary>
    public bool IsReleaseAction { get; set; }
}
public class ReleasePrisonerFromJailCommandHandler : IRequestHandler<ReleasePrisonerFromJailCommand, bool>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;

    public ReleasePrisonerFromJailCommandHandler(IAppDbContext dbContext, IAppRequestContext requestContext)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
    }

    public async Task<bool> Handle(ReleasePrisonerFromJailCommand request, CancellationToken cancellationToken)
    {
        var prison = _requestContext.GetAssociatedPrisonId();
        var prisoner = await _dbContext.Prisoners.AsTracking()
            .Include(x => x.PrisonerFunction)
            .Where(x => x.Id == request.PrisonerId)
            .WhereInPrison(x => x.JailId, prison)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new AppException(CommonExceptionMessages.PrisonerNotFound, true);

        if (!request.IsReleaseAction)
        {
            return false;
        }

        var currentJailId = prisoner.JailId;
        prisoner.JailId = null;
        prisoner.UpdatedOn = AppDateTime.UtcNow;
        prisoner.UpdatedBy = await _requestContext.GetUserId();

        prisoner.PrisonerFunction.LastReleasedOn = AppDateTime.UtcNow;
        prisoner.PrisonerFunction.LastAssociatedJailId = currentJailId;

        await _dbContext.SaveAsync(cancellationToken);
        return true;
    }
}
