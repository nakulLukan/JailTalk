using JailTalk.Shared;
using JailTalk.Shared.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JailTalk.Application.Requests.Identity;

public class JwtTokenRequest : IRequest<ResponseDto<string>>
{
}

public class JwtTokenRequestHandler : IRequestHandler<JwtTokenRequest, ResponseDto<string>>
{
    readonly IConfiguration _configuration;

    public JwtTokenRequestHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<ResponseDto<string>> Handle(JwtTokenRequest request, CancellationToken cancellationToken)
    {
        IdentityModelEventSource.ShowPII = true;
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration[AppSettingKeys.JwtSettingsKey]);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, "tess"),
            new("user", "testuser")
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
        return new ResponseDto<string>(jwt);
    }
}
