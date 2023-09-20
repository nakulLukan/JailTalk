using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Domain.Prison;
using JailTalk.Shared;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prisoner.AccountManagement;
public class AllowOrRevokeUnlimitedCallCommand : IRequest<ResponseDto<UnlimitedCallAction>>
{
    public Guid PrisonerId { get; set; }
    public UnlimitedCallAction Action { get; set; }
}
public class AllowOrRevokeUnlimitedCallCommandHandler : IRequestHandler<AllowOrRevokeUnlimitedCallCommand, ResponseDto<UnlimitedCallAction>>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;

    public AllowOrRevokeUnlimitedCallCommandHandler(IAppDbContext dbContext, IAppRequestContext requestContext)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
    }

    public async Task<ResponseDto<UnlimitedCallAction>> Handle(AllowOrRevokeUnlimitedCallCommand request, CancellationToken cancellationToken)
    {
        var jailId = _requestContext.GetAssociatedPrisonId();
        var prisoneryFunction = await _dbContext.PrisonerFunctions
            .Where(x => x.PrisonerId == request.PrisonerId)
            .WhereInPrison(x => x.Prisoner.JailId, jailId)
            .Select(x => new PrisonerFunction
            {
                Id = x.Id,
                UnlimitedCallsEndsOn = x.UnlimitedCallsEndsOn,
            })
            .FirstOrDefaultAsync(cancellationToken) ?? throw new AppException(CommonExceptionMessages.PrisonerNotFound);

        switch (request.Action)
        {
            case UnlimitedCallAction.Allow:
                _dbContext.Set(prisoneryFunction, (x) => x.UnlimitedCallsEndsOn, AppDateTime.TillEndOfDay);
                break;
            case UnlimitedCallAction.Revoke:
                _dbContext.Set(prisoneryFunction, (x) => x.UnlimitedCallsEndsOn, null);
                break;
            default:
                throw new NotImplementedException();
        }

        await _dbContext.SaveAsync(cancellationToken);
        return new(request.Action);
    }
}

