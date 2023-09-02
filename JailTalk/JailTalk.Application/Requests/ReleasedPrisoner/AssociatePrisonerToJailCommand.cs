using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Shared;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.ReleasedPrisoner;

public class AssociatePrisonerToJailCommand : IRequest<bool>
{
    public Guid PrisonerId { get; set; }
    public int? JailId { get; set; }
}
public class AssociatePrisonerToJailCommandHandler : IRequestHandler<AssociatePrisonerToJailCommand, bool>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;

    public AssociatePrisonerToJailCommandHandler(IAppDbContext dbContext, IAppRequestContext requestContext)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
    }

    public async Task<bool> Handle(AssociatePrisonerToJailCommand request, CancellationToken cancellationToken)
    {
        if (!request.JailId.HasValue)
        {
            throw new AppException("Choose a prison.");
        }

        var prisoner = await _dbContext.Prisoners.AsTracking()
            .FirstOrDefaultAsync(x => x.Id == request.PrisonerId, cancellationToken) ?? throw new AppException(CommonExceptionMessages.PrisonerNotFound);

        var userId = await _requestContext.GetUserId();
        prisoner.JailId = request.JailId.Value;
        prisoner.UpdatedOn = AppDateTime.UtcNow;
        prisoner.UpdatedBy = userId;
        await _dbContext.SaveAsync(cancellationToken);
        return true;
    }
}

