using JailTalk.Domain.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JailTalk.Infrastructure.Persistence.EntityConfigurations.System;
public class ApplicationSettingEFConfig : IEntityTypeConfiguration<ApplicationSetting>
{
    public void Configure(EntityTypeBuilder<ApplicationSetting> builder)
    {
        builder.Property(x => x.Value).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(500);
    }
}