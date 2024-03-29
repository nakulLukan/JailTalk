﻿using JailTalk.Domain.Identity;
using JailTalk.Domain.Prison;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JailTalk.Infrastructure.Persistence.EntityConfigurations.Prison;

public class JailEFConfig : IEntityTypeConfiguration<Jail>
{
    public void Configure(EntityTypeBuilder<Jail> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Code).IsRequired().HasMaxLength(50);
        builder.Property(x => x.AddressId).IsRequired(false);
        builder.Property(x => x.ContactEmailAddress).IsRequired(false).HasMaxLength(100);
        builder.HasOne(x => x.Address)
            .WithMany()
            .HasForeignKey(x => x.AddressId);

        builder.HasMany<AppUser>()
            .WithOne(x => x.Prison)
            .HasForeignKey(x => x.PrisonId);
    }
}
