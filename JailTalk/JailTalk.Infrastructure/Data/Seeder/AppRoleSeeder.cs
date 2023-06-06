using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Infrastructure.Data.Seeder;

public static class AppRoleSeeder
{
    public static async Task SeedRoles(this AppDbContext dbContext)
    {
        string[] roles = new string[]
        {
            "super-admin"
        };
        var existingRoles = await dbContext.Roles.Select(x => x.Name).ToListAsync();
        var roleStore = new RoleStore<IdentityRole>(dbContext);
        foreach (var role in roles.Where(role => !existingRoles.Contains(role)))
        {
            await roleStore.CreateAsync(new IdentityRole
            {
                Name = role,
                NormalizedName = role.Normalize()
            });
        }
    }
}
