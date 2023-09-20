using JailTalk.Domain.Lookup;
using JailTalk.Shared;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

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
                InternalName = "State",
                IsActive = true,
                Name = "States",
            },
            new()
            {
                Id = (int)LookupMasters.Country,
                InternalName = "Country",
                IsActive = true,
                Name = "Countries",
            },
            new()
            {
                Id = (int)LookupMasters.Relationship,
                InternalName = "Relationship",
                IsActive = true,
                Name = "Relationship",
            },
            new()
            {
                Id = (int)LookupMasters.IdProof,
                InternalName = "IdProof",
                IsActive = true,
                Name = "ID Proof",
            },
        };

        foreach (var lookup in lookupMasters)
        {
            modelBuilder.Entity<LookupMaster>().HasData(lookup);
        }

        int lookupDetailId = 1;
        List<LookupDetail> lookupDetails = SeedCountryAndState(ref lookupDetailId);

        // Relationship Seeder
        lookupDetailId = SeedRelationships(lookupDetailId, lookupDetails);

        // Relationship Seeder
        lookupDetailId = SeedIdProofs(lookupDetailId, lookupDetails);

        foreach (var lookupDetail in lookupDetails)
        {
            modelBuilder.Entity<LookupDetail>().HasData(lookupDetail);
        }
    }

    private static List<LookupDetail> SeedCountryAndState(ref int lookupDetailId)
    {
        return new()
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
    }

    private static int SeedRelationships(int lookupDetailId, List<LookupDetail> lookupDetails)
    {
        lookupDetails.AddRange(new LookupDetail[]
                {
            new()
            {
                Id = lookupDetailId++,
                InternalName = "father",
                IsActive = true,
                Order = 1,
                Value = "Father",
                LookupMasterId = (int)LookupMasters.Relationship
            },
            new()
            {
                Id = lookupDetailId++,
                InternalName = "mother",
                IsActive = true,
                Order = 2,
                Value = "Mother",
                LookupMasterId = (int)LookupMasters.Relationship
            },
            new()
            {
                Id = lookupDetailId++,
                InternalName = "brother",
                IsActive = true,
                Order = 3,
                Value = "Brother",
                LookupMasterId = (int)LookupMasters.Relationship
            },
            new()
            {
                Id = lookupDetailId++,
                InternalName = "sister",
                IsActive = true,
                Order = 4,
                Value = "Sister",
                LookupMasterId = (int)LookupMasters.Relationship
            },
            new()
            {
                Id = lookupDetailId++,
                InternalName = "lawyer",
                IsActive = true,
                Order = 5,
                Value = "Lawyer",
                LookupMasterId = (int)LookupMasters.Relationship
            },
            new()
            {
                Id = lookupDetailId++,
                InternalName = "other",
                IsActive = true,
                Order = 6,
                Value = "Others",
                LookupMasterId = (int)LookupMasters.Relationship
            },
                });
        return lookupDetailId;
    }


    private static int SeedIdProofs(int lookupDetailId, List<LookupDetail> lookupDetails)
    {
        lookupDetails.AddRange(new List<LookupDetail>()
        {
            new()
            {
                Id = lookupDetailId++,
                InternalName = "aadhar",
                IsActive = true,
                Order = 1,
                Value = "Aadhar",
                LookupMasterId = (int)LookupMasters.IdProof
            },
            new()
            {
                Id = lookupDetailId++,
                InternalName = "driving_license",
                IsActive = true,
                Order = 2,
                Value = "Driving License",
                LookupMasterId = (int)LookupMasters.IdProof
            },
        });

        return lookupDetailId;
    }
}
