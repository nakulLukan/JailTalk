namespace JailTalk.Application.Dto.Prison;

public class EndCallResultDto
{
    /// <summary>
    /// Balance available amount in rupees.
    /// </summary>
    public float AvailableBalance { get; set; }

    /// <summary>
    /// Net talktime duration. If the reciever didnt pick the call then this value will be 0 minutes.
    /// </summary>
    public string CallDuration { get; set; }
}
