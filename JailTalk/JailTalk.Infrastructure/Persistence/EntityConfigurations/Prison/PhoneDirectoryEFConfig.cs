using JailTalk.Domain.Prison;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JailTalk.Infrastructure.Persistence.EntityConfigurations.Prison;

public class PhoneDirectoryEFConfig : IEntityTypeConfiguration<PhoneDirectory>
{
    public void Configure(EntityTypeBuilder<PhoneDirectory> builder)
    {
        builder.Property(x=>x.PhoneNumber).IsRequired(true).HasMaxLength(15);
        builder.Property(x=>x.CountryCode).IsRequired(true).HasMaxLength(5);
        builder.Property(x=>x.LastBlockedOn).IsRequired(false);
        builder.HasOne(x => x.Prisoner)
            .WithMany()
            .HasForeignKey(x => x.PrisonerId);

        builder.HasOne(x => x.RelativeAddress)
            .WithMany()
            .HasForeignKey(x => x.RelativeAddressId);

        builder.HasMany(x => x.CallHistory)
            .WithOne(x=>x.PhoneDirectory)
            .HasForeignKey(x => x.PhoneDirectoryId);
    }
}
