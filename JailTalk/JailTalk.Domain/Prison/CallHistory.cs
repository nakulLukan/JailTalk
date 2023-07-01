using JailTalk.Shared;

namespace JailTalk.Domain.Prison;
public class CallHistory
{
    public long Id { get; set; }
    public long PhoneDirectoryId { get; set; }
    public DateTimeOffset CallStartedOn { get; set; }
    public DateTimeOffset? EndedOn { get; set; }
    public CallEndReason CallTerminationReason { get; set; }

    public PhoneDirectory PhoneDirectory { get; set; }
}
