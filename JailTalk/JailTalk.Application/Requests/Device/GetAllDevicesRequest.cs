using JailTalk.Application.Contracts.Data;
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

    public GetAllDevicesRequestHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<ResponseDto<List<DeviceListDto>>> Handle(GetAllDevicesRequest request, CancellationToken cancellationToken)
    {
        var devices = await _appDbContext.Devices
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

