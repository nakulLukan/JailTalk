using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Device;
using JailTalk.Shared;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Device;

public class GetDeviceDetailsById : IRequest<DeviceDetailDto>
{
    public Guid DeviceId { get; set; }
}

public class GetDeviceDetailsByIdHandler : IRequestHandler<GetDeviceDetailsById, DeviceDetailDto>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;

    public GetDeviceDetailsByIdHandler(IAppDbContext dbContext, IAppRequestContext requestContext)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
    }

    public async Task<DeviceDetailDto> Handle(GetDeviceDetailsById request, CancellationToken cancellationToken)
    {
        var device = await _dbContext.Devices.AsTracking()
           .WhereInPrison(x => x.JailId, _requestContext.GetAssociatedPrisonId())
           .Where(x => x.Id == request.DeviceId)
           .Select(x => new DeviceDetailDto
           {
               Code = x.Code,
               PrisonId = x.JailId,
               MacAddress = x.MacAddress,
               DeviceSecret = x.DeviceSecretIdentifier.ToString(),
               DeviceId = x.Id,
               IsActive = x.IsActive,
           })
           .FirstOrDefaultAsync(cancellationToken) ?? throw new AppException(CommonExceptionMessages.DeviceNotFound);

        return device;
    }
}
