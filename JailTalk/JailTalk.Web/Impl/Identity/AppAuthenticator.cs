using Microsoft.AspNetCore.Components.Authorization;

namespace JailTalk.Web.Impl.Identity;

public class AppAuthenticator : IAppAuthenticator
{
    readonly AuthenticationStateProvider AuthenticationStateProvider;

    public AppAuthenticator(AuthenticationStateProvider authenticationStateProvider)
    {
        AuthenticationStateProvider = authenticationStateProvider;
    }

    public bool CheckHasPermission(string roles)
    {
        if (roles == "none")
        {
            return true;
        }
        var user = AuthenticationStateProvider.GetAuthenticationStateAsync().Result.User;
        if (!roles.Split(',').Any(role => user.IsInRole(role.Trim())))
        {
            return false;
        }

        return true;
    }
}
