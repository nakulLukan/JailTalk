namespace JailTalk.Application.Dto.Prison;

public class PrisonerListDto
{
    public Guid Id { get; set; }
    public int Serial { get; set; }
    public string Pid { get; set; }
    public string FullName { get; set; }
    public string PrisonCode { get; set; }
}
