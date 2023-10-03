using JailTalk.Domain.Prison;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JailTalk.Infrastructure.Persistence.EntityConfigurations.Prison;

public class JailAccountRechargeRequestEFConfig : IEntityTypeConfiguration<JailAccountRechargeRequest>
{
    public void Configure(EntityTypeBuilder<JailAccountRechargeRequest> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.RechargeSecretHash).IsRequired().HasMaxLength(128);
        builder.Property(x => x.RequestedBy).IsRequired();
        builder.HasOne(x => x.Jail)
            .WithMany()
            .HasForeignKey(x => x.JailId);
    }
}
