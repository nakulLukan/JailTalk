using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Shared;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prisoner;

public class BlockOrUnblockPrisonerCommand : IRequest<bool>
{
    public Guid PrisonerId { get; set; }
    public bool? BlockPermenantly { get; set; }
    public int? NumberOfDaysBlocked { get; set; }
    public bool? UnBlock { get; set; }
}

public class BlockOrUnblockPrisonerCommandHandler : IRequestHandler<BlockOrUnblockPrisonerCommand, bool>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;

    public BlockOrUnblockPrisonerCommandHandler(IAppDbContext dbContext, IAppRequestContext requestContext)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
    }

    public async Task<bool> Handle(BlockOrUnblockPrisonerCommand request, CancellationToken cancellationToken)
    {
        var prisonId = _requestContext.GetAssociatedPrisonId();
        var prisoner = await _dbContext.Prisoners.AsTracking()
            .Include(x => x.PrisonerFunction)
            .WhereInPrison(x => x.JailId, prisonId)
            .SingleOrDefaultAsync(x => x.Id == request.PrisonerId, cancellationToken) ?? throw new AppException(CommonExceptionMessages.PrisonerNotFound, true);

        // While blocking either request.BlockPermenantly must have value or request.NumberOfDaysBlocked should have value.
        // While unblocking these fields should be null
        bool isBlockAction = true;
        if (request.BlockPermenantly.HasValue)
        {
            prisoner.IsBlocked = true;
        }
        else if (request.NumberOfDaysBlocked.HasValue)
        {
            prisoner.IsBlocked = false;
            prisoner.PrisonerFunction.PunishmentEndsOn = AppDateTime.UtcNowAtStartOfTheDay.AddDays(request.NumberOfDaysBlocked.Value);
        }
        else if (request.UnBlock.HasValue)
        {
            prisoner.IsBlocked = false;
            prisoner.PrisonerFunction.PunishmentEndsOn = null;
            isBlockAction = false;
        }

        await _dbContext.SaveAsync(cancellationToken);
        return isBlockAction;
    }
}

