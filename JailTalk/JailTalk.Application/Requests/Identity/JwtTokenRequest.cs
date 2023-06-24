using FluentValidation;
using JailTalk.Application.Contracts.Data;
using JailTalk.Domain.Prison;
using JailTalk.Shared;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JailTalk.Application.Requests.Identity;

public class JwtTokenRequest : IRequest<ResponseDto<string>>
{
    public string MacAddress { get; set; }
    public Guid DeviceSecretIdentifier { get; set; }
}

public class JwtTokenRequestHandler : IRequestHandler<JwtTokenRequest, ResponseDto<string>>
{
    readonly IConfiguration _configuration;
    readonly IAppDbContext _dbContext;

    public JwtTokenRequestHandler(IConfiguration configuration, IAppDbContext dbContext)
    {
        _configuration = configuration;
        _dbContext = dbContext;
    }

    public async Task<ResponseDto<string>> Handle(JwtTokenRequest request, CancellationToken cancellationToken)
    {
        var device = await ValidateDevice(request, cancellationToken);
        string jwt = CreateToken(device);
        return new ResponseDto<string>(jwt);
    }

    private async Task<Device> ValidateDevice(JwtTokenRequest request, CancellationToken cancellationToken)
    {
        var device = await _dbContext.Devices.AsTracking()
                    .Where(x => x.MacAddress == request.MacAddress && x.IsActive)
                    .SingleOrDefaultAsync(cancellationToken);

        if (device is null)
        {
            throw new AppApiException(System.Net.HttpStatusCode.Forbidden, "Unable to recognize device");
        }

        if (device.FailedLoginAttempts > 3 && AppDateTime.UtcNow < device.LockoutEnd.Value)
        {
            throw new AppApiException(System.Net.HttpStatusCode.Forbidden, "Too many login attempts. Please try again later.");
        }

        if (device.DeviceSecretIdentifier != request.DeviceSecretIdentifier)
        {
            if (device.FailedLoginAttempts == 3)
            {
                device.LockoutEnd = AppDateTime.UtcNow.AddDays(1);
            }
            device.FailedLoginAttempts = device.FailedLoginAttempts + 1;

            await _dbContext.SaveAsync(cancellationToken);
            throw new AppApiException(System.Net.HttpStatusCode.Forbidden, "Secret key is invalid");
        }

        device.FailedLoginAttempts = 0;
        device.LastLoggedOn = AppDateTime.UtcNow;
        await _dbContext.SaveAsync(cancellationToken);
        return device;
    }

    private string CreateToken(Device device)
    {
        IdentityModelEventSource.ShowPII = false;
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration[AppSettingKeys.JwtSettingsKey]);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, device.Id.ToString()),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(15),
            Issuer = _configuration[AppSettingKeys.JwtSettingsIssuer],
            Audience = _configuration[AppSettingKeys.JwtSettingsAudience],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);
        return jwt;
    }
}

public class JwtTokenRequestValidator : AbstractValidator<JwtTokenRequest>
{
    public JwtTokenRequestValidator()
    {
        RuleFor(x => x.MacAddress).NotEmpty()
            .Matches("^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$");
    }
}