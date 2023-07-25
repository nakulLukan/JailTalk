using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Device;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Device;

public class GetAllDevicesRequest : IRequest<ResponseDto<List<DeviceListDto>>>
{
}
public class GetAllDevicesRequestHandler : IRequestHandler<GetAllDevicesRequest, ResponseDto<List<DeviceListDto>>>
{
    readonly IAppDbContext _appDbContext;
    readonly IAppRequestContext _requestContext;

    public GetAllDevicesRequestHandler(IAppDbContext appDbContext, IAppRequestContext requestContext)
    {
        _appDbContext = appDbContext;
        _requestContext = requestContext;
    }

    public async Task<ResponseDto<List<DeviceListDto>>> Handle(GetAllDevicesRequest request, CancellationToken cancellationToken)
    {
        var prisonId = _requestContext.GetAssociatedPrisonId();
        var devices = await _appDbContext.Devices
            .WhereInPrison(x => x.JailId, prisonId)
            .OrderBy(x => x.Code)
            .Include(x => x.Jail)
            .ToListAsync(cancellationToken);
        int index = 1;
        return new(devices.Select(x => new DeviceListDto
        {
            Code = x.Code,
            DeviceSecret = x.DeviceSecretIdentifier.ToString().Mask(4, 4),
            IsActive = x.IsActive,
            Prison = x.Jail.Code,
            Serial = index++,
        }).ToList());
    }
}

