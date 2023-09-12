using JailTalk.Domain.Identity;
using JailTalk.Domain.System;

namespace JailTalk.Domain.Prison;
public class PrisonerFaceEncodingMapping
{
    public int Id { get; set; }
    public Guid PrisonerId { get; set; }
    public int ImageId { get; set; }

    public Prisoner Prisoner { get; set; }
    public Attachment Attachment { get; set; }
}
