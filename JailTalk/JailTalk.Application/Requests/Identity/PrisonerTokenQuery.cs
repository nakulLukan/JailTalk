using FluentValidation;
using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Shared;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JailTalk.Application.Requests.Identity;

public class PrisonerTokenQuery : IRequest<ResponseDto<string>>
{
    public string Pid { get; set; }
}

public class PrisonerTokenQueryHandler : IRequestHandler<PrisonerTokenQuery, ResponseDto<string>>
{
    readonly IConfiguration _configuration;
    readonly IAppDbContext _dbContext;
    readonly IDeviceRequestContext _requestContext;
    readonly ILogger<PrisonerTokenQueryHandler> _logger;

    record PrisonerDto(Guid Id, bool IsBlocked, bool IsActive, string FullName, int PrisonId);

    public PrisonerTokenQueryHandler(IConfiguration configuration, IAppDbContext dbContext, IDeviceRequestContext requestContext, ILogger<PrisonerTokenQueryHandler> logger)
    {
        _configuration = configuration;
        _dbContext = dbContext;
        _requestContext = requestContext;
        _logger = logger;
    }

    public async Task<ResponseDto<string>> Handle(PrisonerTokenQuery request, CancellationToken cancellationToken)
    {
        var prisoner = await _dbContext.Prisoners
            .Where(x => x.Pid == request.Pid)
            .Select(x => new PrisonerDto(
                x.Id,
                x.IsBlocked,
                x.IsActive,
                x.FullName,
                x.JailId))
            .FirstOrDefaultAsync(cancellationToken);
        if (prisoner.IsBlocked)
        {
            _logger.LogError("The prisoner is blocked.");
            throw new AppApiException(System.Net.HttpStatusCode.Forbidden, "Inactive record");
        }

        var prisonerId = _requestContext.GetJailId();
        if (prisonerId != prisoner.PrisonId)
        {
            _logger.LogError("Device trying to access prisoner in another jail.");
            throw new AppApiException(System.Net.HttpStatusCode.Forbidden, "Unauthorized access");
        }

        var token = CreateToken(prisoner);
        return new ResponseDto<string>(token);
    }

    private string CreateToken(PrisonerDto device)
    {
        IdentityModelEventSource.ShowPII = false;
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration[AppSettingKeys.JwtSettingsSessionVerificationKey]);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, device.Id.ToString()),
            new(JwtRegisteredClaimNames.Name, device.FullName),
            new(AppClaims.PrisonId, device.PrisonId.ToString()),
        };
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(60),
            Issuer = _configuration[AppSettingKeys.JwtSettingsIssuer],
            Audience = _configuration[AppSettingKeys.JwtSettingsAudience],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);
        return jwt;
    }
}

public class PrisonerTokenQueryValidator : AbstractValidator<PrisonerTokenQuery>
{
    public PrisonerTokenQueryValidator()
    {
        RuleFor(x => x.Pid)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);
    }
}