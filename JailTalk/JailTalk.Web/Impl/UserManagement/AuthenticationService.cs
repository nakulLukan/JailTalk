using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.UserManagement;
using JailTalk.Application.Dto.Identity;
using JailTalk.Domain.Identity;
using JailTalk.Shared.Models;
using JailTalk.Shared.Models.Identity;
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
    readonly IAppDbContext _appDbContext;

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
}
