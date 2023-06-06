using JailTalk.Domain.Identity;
using JailTalk.Shared.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace JailTalk.Infrastructure.Data.Seeder;

public static class AppUserSeeder
{
    public static async Task SeedDefaultUsers(this AppDbContext dbContext, IConfiguration configuration)
    {
        var users = configuration.GetRequiredSection("Identity:DefaultUsers").Get<List<DefaultUserDto>>();

        var existingUsers = await dbContext.Users.Select(x => x.UserName).ToListAsync();
        var userStore = new UserStore<AppUser>(dbContext);
        foreach (var user in users.Where(x => !existingUsers.Contains(x.UserName)))
        {
            AppUser newUser = new AppUser()
            {
                UserName = user.UserName,
                NormalizedUserName = user.UserName.Normalize(),
                Email = user.Email.Normalize(),
            };

            var password = new PasswordHasher<AppUser>();
            var hashed = password.HashPassword(newUser, user.Password);
            newUser.PasswordHash = hashed;
            var result = await userStore.CreateAsync(newUser);
            await userStore.AddToRoleAsync(newUser, user.Role.Normalize());
        }
    }
}
