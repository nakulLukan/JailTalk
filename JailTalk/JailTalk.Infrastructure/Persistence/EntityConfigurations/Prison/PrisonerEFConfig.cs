using JailTalk.Domain.Prison;
using JailTalk.Shared;
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
        builder.Property(x => x.Gender).IsRequired().HasDefaultValue(Gender.Male);
        builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(x => x.IsBlocked).IsRequired().HasDefaultValue(false);
        builder.Property(x => x.JailId).IsRequired(false);
        builder.Property(x => x.PidNumber).IsRequired(true);
        builder.HasIndex(x => x.PidNumber);

        builder.HasOne(x => x.Jail)
            .WithMany()
            .HasForeignKey(x => x.JailId);

        builder.HasMany(x => x.PhoneDirectory)
            .WithOne(x => x.Prisoner)
            .HasForeignKey(x => x.PrisonerId);

        builder.HasOne(x => x.PhoneBalance)
            .WithOne(x => x.Prisoner)
            .HasForeignKey<PhoneBalance>(x => x.PrisonerId);

        builder.HasOne(x => x.PrisonerFunction)
            .WithOne(x => x.Prisoner)
            .HasForeignKey<PrisonerFunction>(x => x.PrisonerId);
    }
}
