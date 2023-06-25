using Microsoft.EntityFrameworkCore;

namespace JailTalk.Infrastructure.Data.Seeder;

public static class SeederBase
{
    public static void SeedData(this ModelBuilder modelBuilder)
    {
        LookupSeeder.SeedData(modelBuilder);
        PrisonSeeder.SeedData(modelBuilder);
    }
}
