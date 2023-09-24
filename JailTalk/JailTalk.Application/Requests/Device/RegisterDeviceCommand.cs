using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Dto.Device;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Device;

public class RegisterDeviceCommand : NewDeviceDto, IRequest<ResponseDto<Guid>>
{
}

public class RegisterDeviceCommandHandler : IRequestHandler<RegisterDeviceCommand, ResponseDto<Guid>>
{
    readonly IAppDbContext _dbContext;

    public RegisterDeviceCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResponseDto<Guid>> Handle(RegisterDeviceCommand request, CancellationToken cancellationToken)
    {
        if (await _dbContext.Devices.AnyAsync(x => request.Code.Trim().ToUpperInvariant() == x.Code.ToUpper(), cancellationToken))
        {
            throw new AppException("A device is already registered with same code. Please choose a unique code.");
        }

        if (await _dbContext.Devices.AnyAsync(x => x.DeviceSecretIdentifier == Guid.Parse(request.DeviceSecret), cancellationToken))
        {
            throw new AppException("Cannot use a secret already in use.");
        }
        
        if (await _dbContext.Devices.AnyAsync(x => x.MacAddress == request.MacAddress, cancellationToken))
        {
            throw new AppException("Another device with same ID already registered.");
        }

        Domain.Prison.Device device = new Domain.Prison.Device()
        {
            Code = request.Code,
            DeviceSecretIdentifier = Guid.Parse(request.DeviceSecret),
            FailedLoginAttempts = 0,
            IsActive = request.IsActive,
            JailId = request.PrisonId.Value,
            LastLoggedOn = null,
            LockoutEnd = null,
            MacAddress = request.MacAddress,
        };

        _dbContext.Devices.Add(device);
        await _dbContext.SaveAsync(cancellationToken);
        return new ResponseDto<Guid>(device.Id);
    }
}
