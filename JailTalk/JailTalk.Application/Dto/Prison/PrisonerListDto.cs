namespace JailTalk.Application.Dto.Prison;

public class PrisonerListDto
{
    public Guid Id { get; set; }
    public string Pid { get; set; }
    public string FullName { get; set; }
    public string PrisonCode { get; set; }
    public string PrisonName { get; set; }
}
