using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Jail;
using JailTalk.Application.Services;
using JailTalk.Shared;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Jail;

public class EditPrisonCommand : EditPrisonDto, IRequest<ResponseDto<int>>
{
}

public class EditPrisonCommandHandler : IRequestHandler<EditPrisonCommand, ResponseDto<int>>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;

    public EditPrisonCommandHandler(IAppDbContext dbContext, IAppRequestContext requestContext)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
    }

    public async Task<ResponseDto<int>> Handle(EditPrisonCommand request, CancellationToken cancellationToken)
    {
        var associatedJailId = _requestContext.GetAssociatedPrisonId();
        var jail = await _dbContext.Jails.AsTracking()
            .WhereInPrison(x => x.Id, associatedJailId)
            .Where(x => x.Id == request.JailId)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new AppException(CommonExceptionMessages.PermissionDenied);

        var userId = await _requestContext.GetUserId();
        jail.UpdatedOn = AppDateTime.UtcNow;
        jail.UpdatedBy = userId;
        jail.ContactEmailAddress = request.ContactEmailAddress;
        jail.Name = request.PrisonName;
        jail.Address = AddressHelper.FromNewAddress(request.Address);

        await _dbContext.SaveAsync(cancellationToken);
        return new(request.JailId);
    }
}
