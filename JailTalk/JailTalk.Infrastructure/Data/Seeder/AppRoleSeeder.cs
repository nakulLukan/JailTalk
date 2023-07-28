using JailTalk.Domain.Identity;
using JailTalk.Shared.Data;
using JailTalk.Shared.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Infrastructure.Data.Seeder;

public static class AppRoleSeeder
{
    public static async Task SeedRoles(this AppDbContext dbContext)
    {
        var existingRoles = await dbContext.Roles.Select(x => x.Name).ToListAsync();
        var roleStore = new RoleStore<AppRole>(dbContext);

        foreach (var role in AppRolesData.Roles.Where(role => !existingRoles.Contains(role.RoleName)))
        {
            await roleStore.CreateAsync(new AppRole
            {
                Name = role.RoleName,
                NormalizedName = role.RoleName.Normalized(),
                Description = role.Description,
            });
        }
    }
}
