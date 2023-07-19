namespace JailTalk.Application.Dto.Prison;

public class RequestCallResultDto
{
    /// <summary>
    /// Unit: Number of seconds that a prisoner can call.
    /// </summary>
    public int AvailableTalkTime { get; set; }

    /// <summary>
    /// Call history tracker id. This value should be used while ending a phone call.
    /// </summary>
    public long CallHistoryId { get; set; }
}
