namespace JailTalk.Application.Dto.Prison;

public class PrisonerContactDetailListDto
{
    public long Id { get; set; }
    public int Serial { get; set; }
    public string ContactNumber { get; set; }
    public string Relationship { get; set; }
    public string RelativeAddress { get; set; }
    public string Status { get; set; }
}
