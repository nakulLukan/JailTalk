using JailTalk.Domain.Lookup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JailTalk.Infrastructure.Persistence.EntityConfigurations.Lookup;

public class AddressBookEFConfig : IEntityTypeConfiguration<AddressBook>
{
    public void Configure(EntityTypeBuilder<AddressBook> builder)
    {
        builder.Property(x => x.HouseName).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.Street).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.City).IsRequired(false).HasMaxLength(30);
        builder.Property(x => x.StateId).IsRequired(false);
        builder.Property(x => x.CountryId).IsRequired(false);

        builder.HasOne(x => x.State)
            .WithMany()
            .HasForeignKey(x => x.StateId);

        builder.HasOne(x => x.Country)
            .WithMany()
            .HasForeignKey(x => x.CountryId);
    }
}
