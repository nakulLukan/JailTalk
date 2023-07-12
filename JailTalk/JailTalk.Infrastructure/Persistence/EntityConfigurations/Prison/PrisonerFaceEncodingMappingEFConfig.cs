using JailTalk.Domain.Prison;
using JailTalk.Domain.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JailTalk.Infrastructure.Persistence.EntityConfigurations.Prison;

public class PrisonerFaceEncodingMappingEFConfig : IEntityTypeConfiguration<PrisonerFaceEncodingMapping>
{
    public void Configure(EntityTypeBuilder<PrisonerFaceEncodingMapping> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasOne(x => x.Prisoner)
            .WithMany(x => x.FaceEncodings)
            .HasForeignKey(x => x.PrisonerId);

        builder.HasOne(x => x.FaceEncoding)
            .WithOne()
            .HasForeignKey<PrisonerFaceEncodingMapping>(x => x.FaceEncodingId);

        builder.HasOne(x => x.Attachment)
            .WithOne()
            .HasForeignKey<PrisonerFaceEncodingMapping>(x => x.ImageId);
    }
}
