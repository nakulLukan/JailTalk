using JailTalk.Application.Dto.Lookup;

namespace JailTalk.Application.Dto.Jail;

public class ViewPrisonDto
{
    public string PrisonCode { get; set; }
    public int JailId { get; set; }
    public string PrisonName { get; set; }
    public NewAddressDto Address { get; set; } = new();
    public string ContactEmailAddress { get; set; }
}
