using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Device;
using JailTalk.Shared;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Device;

public class EditDeviceCommand : EditDeviceDto, IRequest<ResponseDto<Guid>>
{
}

public class EditDeviceCommandHandler : IRequestHandler<EditDeviceCommand, ResponseDto<Guid>>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;

    public EditDeviceCommandHandler(IAppDbContext dbContext, IAppRequestContext requestContext)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
    }

    public async Task<ResponseDto<Guid>> Handle(EditDeviceCommand request, CancellationToken cancellationToken)
    {
        var device = await _dbContext.Devices.AsTracking()
            .WhereInPrison(x => x.JailId, _requestContext.GetAssociatedPrisonId())
            .FirstOrDefaultAsync(x => x.Id == request.DeviceId, cancellationToken) ?? throw new AppException(CommonExceptionMessages.DeviceNotFound);
        await ValidateNewDetails(request, cancellationToken);
        SetNewValues(request, device);
        await _dbContext.SaveAsync(cancellationToken);
        return new ResponseDto<Guid>(device.Id);
    }

    private static void SetNewValues(EditDeviceCommand request, Domain.Prison.Device device)
    {
        if (device.IsActive != request.IsActive)
        {
            device.IsActive = request.IsActive;
        }
        if (device.DeviceSecretIdentifier != Guid.Parse(request.DeviceSecret))
        {
            device.DeviceSecretIdentifier = Guid.Parse(request.DeviceSecret);
        }
        if (device.MacAddress != request.MacAddress)
        {
            device.MacAddress = request.MacAddress;
        }
        if (device.JailId != request.PrisonId)
        {
            device.JailId = request.PrisonId.Value;
        }
    }

    private async Task ValidateNewDetails(EditDeviceCommand request, CancellationToken cancellationToken)
    {
        if (await _dbContext.Devices.AnyAsync(x => x.Id != request.DeviceId
            && x.DeviceSecretIdentifier == Guid.Parse(request.DeviceSecret), cancellationToken))
        {
            throw new AppException("Cannot use a secret already in use.");
        }

        if (await _dbContext.Devices.AnyAsync(x => x.Id != request.DeviceId
            && x.MacAddress == request.MacAddress, cancellationToken))
        {
            throw new AppException("Another device with same mac address already registered.");
        }
    }
}
