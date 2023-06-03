using JailTalk.Domain.Prison;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JailTalk.Infrastructure.Persistence.EntityConfigurations.Prison;

public class PhoneBalanceEFConfig : IEntityTypeConfiguration<PhoneBalance>
{
    public void Configure(EntityTypeBuilder<PhoneBalance> builder)
    {
        builder.HasOne(x => x.Prisoner)
            .WithOne(x=>x.PhoneBalance)
            .HasForeignKey<PhoneBalance>(x => x.PrisonerId);
    }
}
