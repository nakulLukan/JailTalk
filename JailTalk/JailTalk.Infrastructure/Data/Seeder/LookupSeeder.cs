using JailTalk.Domain.Lookup;
using JailTalk.Shared;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Infrastructure.Data.Seeder;

public class LookupSeeder
{
    public static void SeedData(ModelBuilder modelBuilder)
    {
        List<LookupMaster> lookupMasters = new List<LookupMaster>()
        {
            new()
            {
                Id = (int)LookupMasters.State,
                InternalName = "state",
                IsActive = true,
                Name = "States",
            },
            new()
            {
                Id = (int)LookupMasters.Country,
                InternalName = "country",
                IsActive = true,
                Name = "Countries",
            }
        };

        foreach (var lookup in lookupMasters)
        {
            modelBuilder.Entity<LookupMaster>().HasData(lookup);
        }

        int lookupDetailId = 1;
        List<LookupDetail> lookupDetails = new()
        {
            new()
            {
                Id = lookupDetailId++,
                InternalName = "kerala",
                IsActive = true,
                Order = 1,
                Value = "Kerala",
                LookupMasterId = (int)LookupMasters.State
            },
            new()
            {
                Id = lookupDetailId++,
                InternalName = "india",
                IsActive = true,
                Order = 1,
                Value = "India",
                LookupMasterId = (int)LookupMasters.Country
            }

        };

        foreach (var lookupDetail in lookupDetails)
        {
            modelBuilder.Entity<LookupDetail>().HasData(lookupDetail);
        }
    }
}
