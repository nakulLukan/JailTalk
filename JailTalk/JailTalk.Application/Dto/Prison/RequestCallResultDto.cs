namespace JailTalk.Application.Dto.Prison;

public class RequestCallResultDto
{
    /// <summary>
    /// Unit: Number of seconds
    /// </summary>
    public int AvailableTalkTime { get; set; }

    /// <summary>
    /// Call history tracker id
    /// </summary>
    public long CallHistoryId { get; set; }
}
