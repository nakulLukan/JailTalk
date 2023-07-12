using JailTalk.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JailTalk.Infrastructure.Persistence.EntityConfigurations.Identity;

public class AppFaceEncodingEFConfig : IEntityTypeConfiguration<AppFaceEncoding>
{
    public void Configure(EntityTypeBuilder<AppFaceEncoding> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Encoding).IsRequired();
        builder.Property(x => x.EncodingName).IsRequired(false).HasMaxLength(500);
        builder.Property(x => x.LastModifiedBy).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.LastModifiedOn).IsRequired(false);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
    }
}
