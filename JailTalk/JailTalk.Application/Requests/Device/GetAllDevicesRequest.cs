using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Device;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Device;

public class GetAllDevicesRequest : IRequest<PaginatedResponse<DeviceListDto>>
{
    public string SearchValue { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}
public class GetAllDevicesRequestHandler : IRequestHandler<GetAllDevicesRequest, PaginatedResponse<DeviceListDto>>
{
    readonly IAppDbContext _appDbContext;
    readonly IAppRequestContext _requestContext;

    public GetAllDevicesRequestHandler(IAppDbContext appDbContext, IAppRequestContext requestContext)
    {
        _appDbContext = appDbContext;
        _requestContext = requestContext;
    }

    public async Task<PaginatedResponse<DeviceListDto>> Handle(GetAllDevicesRequest request, CancellationToken cancellationToken)
    {
        var prisonId = _requestContext.GetAssociatedPrisonId();
        var devicesQuery = _appDbContext.Devices
            .WhereInPrison(x => x.JailId, prisonId)
            .Where(x => string.IsNullOrEmpty(request.SearchValue) || (x.Code.Contains(request.SearchValue) || x.MacAddress.Contains(request.SearchValue)))
            .OrderBy(x => x.Code);

        var totalCount = await devicesQuery.CountAsync(cancellationToken);
        var devices = await devicesQuery
            .Include(x => x.Jail)
            .Skip(request.Skip.Value)
            .Take(request.Take.Value)
            .ToListAsync(cancellationToken);
        int index = 1;
        return new PaginatedResponse<DeviceListDto>(devices.Select(x => new DeviceListDto
        {
            DeviceId = x.Id,
            Code = x.Code,
            MacAddress = x.MacAddress.ToString().Mask(3, 3),
            IsActive = x.IsActive,
            Prison = $"{x.Jail.Code} ({x.Jail.Name})",
            IsDisabledAtJailLevel = x.Jail.IsSystemTurnedOff,
            Serial = index++,
        }).ToList(), totalCount);
    }
}

