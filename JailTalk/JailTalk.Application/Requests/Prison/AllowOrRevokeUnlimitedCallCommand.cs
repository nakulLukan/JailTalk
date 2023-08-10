using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Domain.Prison;
using JailTalk.Shared;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prison;
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
        var prisoner = await _dbContext.Prisoners
            .Where(x => x.Id == request.PrisonerId)
            .WhereInPrison(x => x.JailId, jailId)
            .Select(x => new Prisoner
            {
                Id = x.Id,
                AllowUnlimitedCallsTill = x.AllowUnlimitedCallsTill,
            })
            .FirstOrDefaultAsync(cancellationToken) ?? throw new AppException(CommonExceptionMessages.PrisonerNotFound);
        
        switch (request.Action)
        {
            case UnlimitedCallAction.Allow:
                _dbContext.Set(prisoner, (x) => x.AllowUnlimitedCallsTill, AppDateTime.TillEndOfDay);
                break;
            case UnlimitedCallAction.Revoke:
                _dbContext.Set(prisoner, (x) => x.AllowUnlimitedCallsTill, null);
                break;
            default:
                throw new NotImplementedException();
        }

        await _dbContext.SaveAsync(cancellationToken);
        return new(request.Action);
    }
}

