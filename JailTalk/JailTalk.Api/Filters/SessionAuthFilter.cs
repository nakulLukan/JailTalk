using JailTalk.Shared;
using JailTalk.Shared.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace JailTalk.Api.Filters;

public class SessionAuthFilter : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var _configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var _logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<SessionAuthFilter>>();
        var securityKey = Encoding.UTF8.GetBytes(_configuration[AppSettingKeysConstant.JwtSettingsSessionVerificationKey]);
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidIssuer = _configuration[AppSettingKeysConstant.JwtSettingsIssuer],
            ValidAudience = _configuration[AppSettingKeysConstant.JwtSettingsAudience],
            IssuerSigningKey = new SymmetricSecurityKey(securityKey),

            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
        };

        try
        {
            if (!context.HttpContext.Request.Headers.ContainsKey(HttpHeader.SessionToken))
            {
                _logger.LogError("The request does not contain a session token");
                context.Result = GetUnauthorizedObjectResult();
                return;
            }

            var jwtToken = context.HttpContext.Request.Headers[HttpHeader.SessionToken];
            var principal = tokenHandler.ValidateToken(jwtToken, validationParameters, out var validatedToken);
        }
        catch (SecurityTokenException ex)
        {
            _logger.LogError("Session Token Validation Failed: {message}", ex.Message);
            context.Result = GetUnauthorizedObjectResult();
        }
    }

    private UnauthorizedObjectResult GetUnauthorizedObjectResult()
    {
        return new UnauthorizedObjectResult(new ProblemDetails
        {
            Title = "Unauthorized",
            Status = (int)HttpStatusCode.Unauthorized,
            Detail = "Session Token is Invalid"
        });
    }
}
