using JailTalk.Domain.Identity;
using JailTalk.Web.Impl.UserManagement;
using Microsoft.AspNetCore.Identity;

namespace JailTalk.Web.Middlewares;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, SignInManager<AppUser> signInMgr)
    {
        if (context.Request.Path == "/login" && context.Request.Query.ContainsKey("key"))
        {
            var key = Guid.Parse(context.Request.Query["key"]);
            var info = AuthenticationService.Logins[key];

            var result = await signInMgr.PasswordSignInAsync(info.UserName, info.Password, false, lockoutOnFailure: true);
            info.Password = null;
            if (result.Succeeded)
            {
                AuthenticationService.Logins.Remove(key);
                context.Response.Redirect("/");
                return;
            }
            else if (result.RequiresTwoFactor)
            {
                //TODO: redirect to 2FA razor component
                context.Response.Redirect("/loginwith2fa/" + key);
                return;
            }
            else
            {
                //TODO: Proper error handling
                context.Response.Redirect("/loginfailed");
                return;
            }
        }
        else
        {
            await _next.Invoke(context);
        }
    }
}