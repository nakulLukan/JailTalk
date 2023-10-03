using JailTalk.Domain.Prison;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JailTalk.Infrastructure.Persistence.EntityConfigurations.Prison;

public class JailAccountBalanceEFConfig : IEntityTypeConfiguration<JailAccountBalance>
{
    public void Configure(EntityTypeBuilder<JailAccountBalance> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.HasOne(x => x.Jail)
            .WithOne(x => x.AccountBalance)
            .HasForeignKey<JailAccountBalance>(x => x.JailId);
    }
}
