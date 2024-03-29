﻿using FluentValidation;
using JailTalk.Application.Contracts.Data;
using JailTalk.Shared;
using JailTalk.Shared.Constants;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace JailTalk.Application.Requests.Identity;

public class DeviceTokenQuery : IRequest<ApiResponseDto<string>>
{
    /// <summary>
    /// Device android id
    /// </summary>
    public string MacAddress { get; set; }

    /// <summary>
    /// Device secret key shared with the system
    /// </summary>
    public Guid DeviceSecretIdentifier { get; set; }
}

public class JwtTokenRequestHandler : IRequestHandler<DeviceTokenQuery, ApiResponseDto<string>>
{
    readonly IConfiguration _configuration;
    readonly IAppDbContext _dbContext;

    public JwtTokenRequestHandler(IConfiguration configuration, IAppDbContext dbContext)
    {
        _configuration = configuration;
        _dbContext = dbContext;
    }

    public async Task<ApiResponseDto<string>> Handle(DeviceTokenQuery request, CancellationToken cancellationToken)
    {
        var device = await ValidateDevice(request, cancellationToken);
        string jwt = CreateToken(device);
        return new ApiResponseDto<string>(jwt);
    }

    private async Task<Domain.Prison.Device> ValidateDevice(DeviceTokenQuery request, CancellationToken cancellationToken)
    {
        var device = await _dbContext.Devices.AsTracking()
                    .Where(x => x.MacAddress == request.MacAddress && x.IsActive)
                    .SingleOrDefaultAsync(cancellationToken);

        if (device is null)
        {
            throw new AppApiException(HttpStatusCode.Forbidden, "DT-0001", "Unable to recognize device");
        }

        if (device.FailedLoginAttempts > 3 && AppDateTime.UtcNow < device.LockoutEnd.Value)
        {
            throw new AppApiException(HttpStatusCode.Forbidden, "DT-0002", "Too many login attempts. Please try again later.");
        }

        if (device.DeviceSecretIdentifier != request.DeviceSecretIdentifier)
        {
            if (device.FailedLoginAttempts == 3)
            {
                device.LockoutEnd = AppDateTime.UtcNow.AddDays(1);
            }
            device.FailedLoginAttempts = device.FailedLoginAttempts + 1;

            await _dbContext.SaveAsync(cancellationToken);
            throw new AppApiException(HttpStatusCode.Forbidden, "DT-0003", "Secret key is invalid");
        }

        device.FailedLoginAttempts = 0;
        device.LastLoggedOn = AppDateTime.UtcNow;
        await _dbContext.SaveAsync(cancellationToken);
        return device;
    }

    private string CreateToken(Domain.Prison.Device device)
    {
        IdentityModelEventSource.ShowPII = false;
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration[AppSettingKeysConstant.JwtSettingsKey]);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, device.Id.ToString()),
            new(JwtRegisteredClaimNames.Name, device.Code),
            new(AppClaims.PrisonId, device.JailId.ToString()),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(15),
            Issuer = _configuration[AppSettingKeysConstant.JwtSettingsIssuer],
            Audience = _configuration[AppSettingKeysConstant.JwtSettingsAudience],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);
        return jwt;
    }
}

public class JwtTokenRequestValidator : AbstractValidator<DeviceTokenQuery>
{
    public JwtTokenRequestValidator()
    {
        RuleFor(x => x.MacAddress).NotEmpty()
            .Matches(RegularExpressionPatternConstant.MacAddress);
    }
}