using JailTalk.Domain.Prison;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JailTalk.Infrastructure.Persistence.EntityConfigurations.Prison;

public class CallHistoryEFConfig : IEntityTypeConfiguration<CallHistory>
{
    public void Configure(EntityTypeBuilder<CallHistory> builder)
    {
        builder.HasOne(x => x.PhoneDirectory)
            .WithMany(x => x.CallHistory)
            .HasForeignKey(x => x.PhoneDirectoryId);
    }
}
