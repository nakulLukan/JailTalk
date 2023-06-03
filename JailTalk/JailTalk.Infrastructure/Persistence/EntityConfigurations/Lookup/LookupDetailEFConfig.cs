using JailTalk.Domain.Lookup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JailTalk.Infrastructure.Persistence.EntityConfigurations.Lookup;

public class LookupDetailEFConfig : IEntityTypeConfiguration<LookupDetail>
{
    public void Configure(EntityTypeBuilder<LookupDetail> builder)
    {
        builder.Property(x => x.InternalName).IsRequired().HasMaxLength(20);
        builder.Property(x => x.Value).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Order).IsRequired(false);

        builder.HasOne(x => x.LookupMaster)
            .WithMany(x => x.LookupDetails)
            .HasForeignKey(x => x.LookupMasterId);
    }
}
