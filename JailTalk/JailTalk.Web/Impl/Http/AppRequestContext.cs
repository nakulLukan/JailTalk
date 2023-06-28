using JailTalk.Application.Contracts.Http;
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
        if(authState is null)
        {
            authState = await _authStateProvider.GetAuthenticationStateAsync();
        }
        return authState.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
    }
}
