using JailTalk.Domain.Prison;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JailTalk.Infrastructure.Persistence.EntityConfigurations.Prison;

public class PrisonerEFConfig : IEntityTypeConfiguration<Prisoner>
{
    public void Configure(EntityTypeBuilder<Prisoner> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Pid).IsRequired(true).HasMaxLength(100);
        builder.HasIndex(x => x.Pid);

        builder.Property(x => x.FirstName).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.MiddleName).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.LastName).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.FullName).IsRequired(true).HasMaxLength(150);
        builder.Property(x => x.AddressId).IsRequired(false);
        builder.HasOne(x => x.Address)
            .WithMany()
            .HasForeignKey(x => x.AddressId);

        builder.HasOne(x => x.Jail)
            .WithMany()
            .HasForeignKey(x => x.JailId);

        builder.HasMany(x => x.PhoneDirectory)
            .WithOne(x => x.Prisoner)
            .HasForeignKey(x => x.PrisonerId);

        builder.HasOne(x => x.PhoneBalance)
            .WithOne(x => x.Prisoner)
            .HasForeignKey<PhoneBalance>(x => x.PrisonerId);
    }
}
