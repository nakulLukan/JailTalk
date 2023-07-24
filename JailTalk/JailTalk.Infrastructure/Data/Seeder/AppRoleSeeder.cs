using JailTalk.Domain.Identity;
using JailTalk.Shared.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Infrastructure.Data.Seeder;

public static class AppRoleSeeder
{
    public static async Task SeedRoles(this AppDbContext dbContext)
    {
        // Index 0: Name of the role.
        // Index 1: Description of the role.
        string[][] roles = new string[][]
        {
            new string[]{ "super-admin", "Does have full access to the application." },
            new string[]{ "supervisor", "Does have full access to the application. The data is restricted to the users associated prison. The user cannot view data of other prisons." },
            new string[]{ "subordinate", "Does have only limited access to the application. The data is restricted to the users associated prison." },
        };
        var existingRoles = await dbContext.Roles.Select(x => x.Name).ToListAsync();
        var roleStore = new RoleStore<AppRole>(dbContext);

        // Role iterator
        int i = 0;
        foreach (var role in roles.Where(role => !existingRoles.Contains(role[i])))
        {
            await roleStore.CreateAsync(new AppRole
            {
                Name = role[0],
                NormalizedName = role[0].Normalized(),
                Description = role[1]
            });
        }
    }
}
