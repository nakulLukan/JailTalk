namespace JailTalk.Application.Dto.Prison;

public class PrisonerSecondaryDetailDto
{
    public Guid PrisonerId { get; set; }

    public bool IsBlocked { get; set; }
    public DateTimeOffset? RetrictionEndsOn { get; set; }

    public bool IsReleased { get; set; }
    public DateTimeOffset? LastReleasedOn { get; set; }
    public string LastAssociatedJailCode { get; set; }

    public bool IsUnlimitedCallEnabled { get; set; }

}
