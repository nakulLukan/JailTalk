using JailTalk.Domain.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JailTalk.Infrastructure.Persistence.EntityConfigurations.System;

public class AttachmentEFConfig : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Data).IsRequired(false);
        builder.Property(x => x.IsBlob);
        builder.Property(x => x.RelativeFilePath).IsRequired(false).HasMaxLength(500);
        builder.Property(x => x.FileName).IsRequired(true).HasMaxLength(100);
    }
}
