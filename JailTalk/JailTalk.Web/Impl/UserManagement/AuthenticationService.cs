using JailTalk.Application.Contracts.UserManagement;
using JailTalk.Application.Dto.Identity;
using JailTalk.Domain.Identity;
using JailTalk.Shared;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Models;
using JailTalk.Shared.Models.Identity;
using JailTalk.Shared.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace JailTalk.Web.Impl.UserManagement;

public class AuthenticationService : IAuthenticationService
{
    public static IDictionary<Guid, LoginInfo> Logins { get; private set; }
        = new ConcurrentDictionary<Guid, LoginInfo>();

    readonly IServiceScopeFactory _serviceScopeFactory;

    public AuthenticationService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<UserSignInResultDto> SignInUser(string email, string password)
    {
        using var scope = _serviceScopeFactory.CreateScope();
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
        if (result.Succeeded)
        {
            // Add a custom claim "Role" to represent the user's role.
            // This claim can be used for authorization purposes later.
            var userRoles = await userInManager.GetRolesAsync(user);
            var roleClaim = new Claim(ClaimTypes.Role, userRoles.First());
            await signInManager.UserManager.AddClaimAsync(user, roleClaim);

            // Add associated prison claim. This can be used restrict data belonging to that prison.
            var associatedPrisonClaim = new Claim(AppClaims.PrisonId, user.PrisonId?.ToString() ?? 0.ToString());
            await signInManager.UserManager.AddClaimAsync(user, associatedPrisonClaim);
        }
        return new UserSignInResultDto
        {
            IsLockedOut = result.IsLockedOut,
            IsNotAllowed = result.IsNotAllowed,
            RequiresTwoFactor = result.RequiresTwoFactor,
            Succeeded = result.Succeeded,
            UserName = user.UserName
        };
    }

    public async Task<List<RolesListDto>> GetAllRoles()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
        return await roleManager.Roles.Select(x => new RolesListDto
        {
            RoleName = x.Name,
            RoleId = x.Id,
            RoleDescription = x.Description,
        }).ToListAsync();
    }

    public async Task SignOut()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<AppUser>>();
        await signInManager.SignOutAsync();
    }

    public async Task<string> AddUser(AddUserAccountDto accountDetails)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        AppUser newUser = new AppUser()
        {
            UserName = accountDetails.Username,
            NormalizedUserName = accountDetails.Username.Normalized(),
            Email = accountDetails.Email,
            NormalizedEmail = accountDetails.Email.Normalized(),
            LockoutEnabled = true,
            PrisonId = accountDetails.PrisonId,
            FullName = accountDetails.FullName,
        };

        var password = new PasswordHasher<AppUser>();
        var hashed = password.HashPassword(newUser, accountDetails.Password);
        newUser.PasswordHash = hashed;
        var result = await userManager.CreateAsync(newUser);

        if (!string.IsNullOrEmpty(accountDetails.RoleName))
        {
            await userManager.AddToRoleAsync(newUser, accountDetails.RoleName.Normalized());
        }

        return newUser.Id;
    }

    public async Task<bool> LockUserAccount(string userId, bool lockAccount)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new AppException(CommonExceptionMessages.UserNotFound);
        }

        if (lockAccount)
        {
            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.MaxValue;
        }
        else
        {
            user.LockoutEnd = null;
        }
        await userManager.UpdateAsync(user);
        return true;
    }
}
