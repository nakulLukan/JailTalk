namespace JailTalk.Application.Dto.Prison;

public class PrisonerContactDetailDto
{
    public long Id { get; set; }
    public string PhoneNumber { get; set; }
    public string CountryCode { get; set; }
    public int? RelativeTypeId { get; set; }
    public string Name { get; set; }
}
