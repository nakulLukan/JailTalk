namespace JailTalk.Application.Dto.ReleasedPrisoner;

public class ReleasedPrisonerListDto
{
    public Guid PrisonerId { get; set; }
    public string Pid { get; set; }
    public string FullName { get; set; }
    public string LastReleasedOn { get; set; }
}
