namespace JailTalk.Application.Dto.Prison;

public class PrisonerLastAssociatedJailDetailDto
{
    public string LastAssociatedPrison { get; set; }
    public string LastReleasedOn { get; set; }

    /// <summary>
    /// Flag to indicate whether this prisoner is associated to any jail. If yes then the associate button should be disabled.
    /// </summary>
    public bool IsPrisonerAssociatedToJail { get; set; }
}
