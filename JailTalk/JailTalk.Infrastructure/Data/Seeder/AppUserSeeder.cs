using JailTalk.Domain.Identity;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace JailTalk.Infrastructure.Data.Seeder;

public static class AppUserSeeder
{
    public static async Task SeedDefaultUsers(this AppDbContext dbContext,
                                              IConfiguration configuration,
                                              UserManager<AppUser> userManager)
    {
        var users = configuration.GetRequiredSection("Identity:DefaultUsers").Get<List<DefaultUserDto>>();

        var existingUsers = await dbContext.Users.Select(x => x.UserName).ToListAsync();
        foreach (var user in users.Where(x => !existingUsers.Contains(x.UserName)))
        {
            AppUser newUser = new AppUser()
            {
                UserName = user.UserName,
                NormalizedUserName = user.UserName.Normalized(),
                Email = user.Email,
                NormalizedEmail = user.Email.Normalized(),
                LockoutEnabled = true,
            };
            var password = new PasswordHasher<AppUser>();
            var hashed = password.HashPassword(newUser, user.Password);
            newUser.PasswordHash = hashed;
            var result = await userManager.CreateAsync(newUser);
            Serilog.Log.Logger.Information("User : {user} add status: {status}", user.UserName, result.Succeeded);
            result = await userManager.AddToRoleAsync(newUser, user.Role.Normalized());
            Serilog.Log.Logger.Information("User assigned to role: {role} add status: {status}", user.Role, result.Succeeded);

        }
    }
}
