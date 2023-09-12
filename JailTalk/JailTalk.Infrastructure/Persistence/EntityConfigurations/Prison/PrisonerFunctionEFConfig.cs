using JailTalk.Domain.Prison;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JailTalk.Infrastructure.Persistence.EntityConfigurations.Prison;

public class PrisonerFunctionEFConfig : IEntityTypeConfiguration<PrisonerFunction>
{
    public void Configure(EntityTypeBuilder<PrisonerFunction> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.PrisonerId).IsRequired();
        builder.Property(x => x.LastAssociatedJailId).IsRequired(false);
        builder.Property(x => x.UnlimitedCallsEndsOn).IsRequired(false);
        builder.Property(x => x.LastReleasedOn).IsRequired(false);
        builder.Property(x => x.PunishmentEndsOn).IsRequired(false);

        builder.HasOne(x => x.Prisoner)
            .WithOne(x => x.PrisonerFunction)
            .HasForeignKey<PrisonerFunction>(x => x.PrisonerId);

        builder.HasOne(x => x.LastAssociatedJail)
            .WithMany()
            .HasForeignKey(x => x.LastAssociatedJailId);

        builder.HasOne(x => x.DpAttachment)
            .WithMany()
            .HasForeignKey(x => x.DpAttachmentId);
    }
}
