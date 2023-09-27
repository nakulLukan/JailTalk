using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Shared;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Jail;

public class TurnfOnOffJailDeviceCommand : IRequest<ResponseDto<bool>>
{
    public int JailId { get; set; }
    public bool TurnOffDevice { get; set; }
}

public class TurnfOnOffJailDeviceCommandHandler : IRequestHandler<TurnfOnOffJailDeviceCommand, ResponseDto<bool>>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;

    public TurnfOnOffJailDeviceCommandHandler(IAppDbContext dbContext, IAppRequestContext requestContext)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
    }

    public async Task<ResponseDto<bool>> Handle(TurnfOnOffJailDeviceCommand request, CancellationToken cancellationToken)
    {
        var jail = await _dbContext.Jails.AsTracking()
            .WhereInPrison(x => x.Id, _requestContext.GetAssociatedPrisonId())
            .Where(x => x.Id == request.JailId)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new AppException(CommonExceptionMessages.PermissionDenied);

        jail.IsSystemTurnedOff = request.TurnOffDevice;
        jail.UpdatedOn = AppDateTime.UtcNow;
        jail.UpdatedBy = await _requestContext.GetUserId();

        await _dbContext.SaveAsync(cancellationToken);
        return new(request.TurnOffDevice);
    }
}
