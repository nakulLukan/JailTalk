using JailTalk.Application.Contracts.UserManagement;
using JailTalk.Domain.Identity;
using JailTalk.Shared.Models;
using Microsoft.AspNetCore.Identity;

namespace JailTalk.Web.Impl.UserManagement;

public class AuthenticationService : IAuthenticationService
{
    readonly SignInManager<AppUser> _signInManager;

    public AuthenticationService(SignInManager<AppUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<UserSignInResultDto> SignInUser(string username, string password)
    {
        var user = await _signInManager.PasswordSignInAsync(username, password, false, true);
        return new UserSignInResultDto
        {
            IsLockedOut = user.IsLockedOut,
            IsNotAllowed = user.IsNotAllowed,
            RequiresTwoFactor = user.RequiresTwoFactor,
            Succeeded = user.Succeeded,
        };
    }
}
