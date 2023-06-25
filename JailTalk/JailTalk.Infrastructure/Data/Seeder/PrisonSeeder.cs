using JailTalk.Domain.Prison;
using JailTalk.Shared.Utilities;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Infrastructure.Data.Seeder;

public static class PrisonSeeder
{
    public static void SeedData(ModelBuilder modelBuilder)
    {
        List<Jail> jails = new();
        jails.Add(new()
        {
            Id = 1,
            Name = "Ponnani Sub Jail",
            Code = "MLP-PN-SJ",
            CreatedBy = null,
            CreatedOn = AppDateTime.UtcNow,
            UpdatedBy = null,
            UpdatedOn = AppDateTime.UtcNow,
            AddressId = null
        });

        foreach (var seed in jails)
        {
            modelBuilder.Entity<Jail>().HasData(seed);
        }
    }
}
