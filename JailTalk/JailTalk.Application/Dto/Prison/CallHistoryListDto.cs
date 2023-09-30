namespace JailTalk.Application.Dto.Prison;

public class CallHistoryListDto
{
    public long Id { get; set; }
    public int Serial { get; set; }
    public string Callee { get; set; }
    public string ContactNumber { get; set; }
    public string CallStartedOn { get; set; }
    public string CallEndedOn { get; set; }
    public string CallDuration { get; set; }
    public string CallEndReason { get; set; }

    /// <summary>
    /// True: Call recording clip is available for this call record.
    /// False: Does not have clip
    /// Null: Call clip cannot be accessed.
    /// </summary>
    public bool? CallRecordingState { get; set; }
}
