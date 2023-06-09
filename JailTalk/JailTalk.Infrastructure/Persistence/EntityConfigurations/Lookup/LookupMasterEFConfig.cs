﻿using JailTalk.Domain.Lookup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JailTalk.Infrastructure.Persistence.EntityConfigurations.Lookup;

public class LookupMasterEFConfig : IEntityTypeConfiguration<LookupMaster>
{
    public void Configure(EntityTypeBuilder<LookupMaster> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.InternalName).IsRequired().HasMaxLength(15);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);

        builder.HasMany(x => x.LookupDetails)
            .WithOne(x => x.LookupMaster)
            .HasForeignKey(x => x.LookupMasterId);
    }
}
