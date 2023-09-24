using JailTalk.Domain.System;
using JailTalk.Shared;

namespace JailTalk.Domain.Prison;
public class CallHistory
{
    public long Id { get; set; }
    public long PhoneDirectoryId { get; set; }
    public DateTimeOffset CallStartedOn { get; set; }
    public DateTimeOffset? EndedOn { get; set; }
    public CallEndReason CallTerminationReason { get; set; }
    public int? CallRecordingAttachmentId { get; set; }

    /// <summary>
    /// Jail to which this call history record is associated
    /// </summary>
    public int AssociatedPrisonId { get; set; }

    public PhoneDirectory PhoneDirectory { get; set; }
    public Attachment CallRecordingAttachment { get; set; }
    public Jail AssociatedPrison { get; set; }

}
