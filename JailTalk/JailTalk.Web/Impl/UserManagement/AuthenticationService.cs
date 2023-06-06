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

    readonly SignInManager<AppUser> _signInManager;
    readonly UserManager<AppUser> _userInManager;

    public AuthenticationService(SignInManager<AppUser> signInManager, UserManager<AppUser> userInManager)
    {
        _signInManager = signInManager;
        _userInManager = userInManager;
    }

    public async Task<UserSignInResultDto> SignInUser(string email, string password)
    {
        var user = await _userInManager.FindByEmailAsync(email);
        if (user == null)
        {
            return new UserSignInResultDto()
            {
                Succeeded = false
            };
        }
        var result = await _signInManager.CheckPasswordSignInAsync(user, password, true);
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
