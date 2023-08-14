using JailTalk.Application.Contracts.Http;
using JailTalk.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace JailTalk.Web.Impl.Http;

public class AppRequestContext : IAppRequestContext
{
    protected AuthenticationStateProvider _authStateProvider;

    AuthenticationState authState;

    public AppRequestContext(AuthenticationStateProvider authStateProvider)
    {
        _authStateProvider = authStateProvider;
    }

    public async Task<string> GetUserId()
    {
        if (authState is null)
        {
            authState = await _authStateProvider.GetAuthenticationStateAsync();
        }
        return authState.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
    }

    public async Task<string> GetUserName()
    {
        if (authState is null)
        {
            authState = await _authStateProvider.GetAuthenticationStateAsync();
        }

        return authState.User.Claims.First(x => x.Type == ClaimTypes.Name).Value;
    }

    public int GetAssociatedPrisonId()
    {
        if (authState is null)
        {
            authState = _authStateProvider.GetAuthenticationStateAsync().Result;
        }
        return int.Parse(authState.User.Claims.First(x => x.Type == AppClaims.PrisonId).Value);
    }
}
