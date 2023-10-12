using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Device;
using JailTalk.Shared;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Device;

public class GetDeviceBasicDetailQuery : IRequest<DeviceBasicDetailDto>
{
}

public class GetDeviceBasicDetailQueryHandler : IRequestHandler<GetDeviceBasicDetailQuery, DeviceBasicDetailDto>
{
    readonly IAppDbContext _dbContext;
    readonly IDeviceRequestContext _requestContext;

    public GetDeviceBasicDetailQueryHandler(IAppDbContext dbContext, IDeviceRequestContext requestContext)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
    }

    public async Task<DeviceBasicDetailDto> Handle(GetDeviceBasicDetailQuery request, CancellationToken cancellationToken)
    {
        var deviceId = _requestContext.GetDeviceId();
        var deviceBasicDetail = await _dbContext.Devices
           .Where(x => x.Id == deviceId)
           .Select(x => new DeviceBasicDetailDto
           {
               Code = x.Code,
               PrisonCode = x.Jail.Code,
               PrisonName = x.Jail.Name,
           })
           .FirstOrDefaultAsync(cancellationToken) ?? throw new AppException(CommonExceptionMessages.DeviceNotFound);

        return deviceBasicDetail;
    }
}
