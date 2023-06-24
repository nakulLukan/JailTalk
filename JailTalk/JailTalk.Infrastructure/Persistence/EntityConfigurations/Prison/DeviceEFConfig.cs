using JailTalk.Domain.Prison;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JailTalk.Infrastructure.Persistence.EntityConfigurations.Prison;

public class DeviceEFConfig : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.DeviceSecretIdentifier).IsRequired();
        builder.Property(x => x.Code).IsRequired().HasMaxLength(50);
        builder.Property(x => x.MacAddress).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.MacAddress);
        builder.Property(x => x.LastLoggedOn).IsRequired(false);
        builder.Property(x => x.LockoutEnd).IsRequired(false);

        builder.HasOne(x => x.Jail)
            .WithMany()
            .HasForeignKey(x => x.JailId);
    }
}
