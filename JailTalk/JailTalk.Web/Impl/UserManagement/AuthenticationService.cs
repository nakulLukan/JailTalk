using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.UserManagement;
using JailTalk.Domain.Identity;
using JailTalk.Shared.Models;
using JailTalk.Shared.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Concurrent;

namespace JailTalk.Web.Impl.UserManagement;

public class AuthenticationService : IAuthenticationService
{
    public static IDictionary<Guid, LoginInfo> Logins { get; private set; }
        = new ConcurrentDictionary<Guid, LoginInfo>();

    readonly IServiceScopeFactory _serviceScopeFactory;
    readonly IAppDbContext _appDbContext;

    public AuthenticationService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<UserSignInResultDto> SignInUser(string email, string password)
    {
        using var scope =_serviceScopeFactory.CreateScope();
        var userInManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<AppUser>>();
        var user = await userInManager.FindByEmailAsync(email);
        if (user == null)
        {
            return new UserSignInResultDto()
            {
                Succeeded = false
            };
        }
        var result = await signInManager.CheckPasswordSignInAsync(user, password, true);
        return new UserSignInResultDto
        {
            IsLockedOut = result.IsLockedOut,
            IsNotAllowed = result.IsNotAllowed,
            RequiresTwoFactor = result.RequiresTwoFactor,
            Succeeded = result.Succeeded,
            UserName = user.UserName
        };
    }
}
