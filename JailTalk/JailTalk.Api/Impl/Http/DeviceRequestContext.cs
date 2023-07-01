using JailTalk.Application.Contracts.Http;
using JailTalk.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JailTalk.Api.Impl.Http;

public class DeviceRequestContext : IDeviceRequestContext
{
    readonly IHttpContextAccessor _contextAccessor;

    public DeviceRequestContext(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public Guid GetDeviceId()
    {
        var claim = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        if (claim == null)
        {
            throw new ArgumentException("Device Id Claim Not Found");
        }

        return Guid.Parse(claim.Value);
    }

    public int GetJailId()
    {
        var claim = _contextAccessor.HttpContext.User.Claims.
            FirstOrDefault(x => x.Type == AppClaims.PrisonId);
        if (claim == null)
        {
            throw new ArgumentException("Prison Id Claim Not Found");
        }

        return int.Parse(claim.Value);
    }

    public Guid GetPrisonerId()
    {
        if (!_contextAccessor.HttpContext.Request.Headers.ContainsKey(HttpHeader.SessionToken))
        {
            throw new ArgumentException("Invalid header");
        }
        var token = _contextAccessor.HttpContext.Request.Headers[HttpHeader.SessionToken];
        var tokenHandler = new JwtSecurityTokenHandler();
        var parsedToken = tokenHandler.ReadJwtToken(token);
        var claim = parsedToken.Claims.
            FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);
        if (claim == null)
        {
            throw new ArgumentException("Prisoner Id Claim Not Found");
        }

        return Guid.Parse(claim.Value);
    }
}
