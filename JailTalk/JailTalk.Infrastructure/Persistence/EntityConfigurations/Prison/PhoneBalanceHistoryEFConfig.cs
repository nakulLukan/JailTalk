using JailTalk.Domain.Prison;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JailTalk.Infrastructure.Persistence.EntityConfigurations.Prison;

public class PhoneBalanceHistoryEFConfig : IEntityTypeConfiguration<PhoneBalanceHistory>
{
    public void Configure(EntityTypeBuilder<PhoneBalanceHistory> builder)
    {
        builder.Property(x=>x.CallRequestId).IsRequired(false);
        builder.Property(x=>x.RechargedByUserId).IsRequired(false);
        builder.HasOne(x => x.Prisoner)
            .WithMany()
            .HasForeignKey(x => x.PrisonerId);

        builder.HasOne(x => x.CallRequest)
            .WithOne()
            .HasForeignKey<PhoneBalanceHistory>(x => x.CallRequestId);
    }
}
