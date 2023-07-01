using JailTalk.Domain.Prison;
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
            CreatedOn = new DateTimeOffset(2023, 7, 1, 0, 0, 0, TimeSpan.Zero),
            UpdatedBy = null,
            UpdatedOn = new DateTimeOffset(2023, 7, 1, 0, 0, 0, TimeSpan.Zero),
            AddressId = null
        });

        foreach (var seed in jails)
        {
            modelBuilder.Entity<Jail>().HasData(seed);
        }
    }
}
