using JailTalk.Application.Dto.Lookup;

namespace JailTalk.Application.Dto.Prison;

public class PrisonerContactDetailDto
{
    public long Id { get; set; }
    public Guid PrisonerId { get; set; }
    public string CountryCode { get; set; }
    public string PhoneNumber { get; set; }
    public string Name { get; set; }
    public int? RelationshipId { get; set; }
    public NewAddressDto RelativeAddress { get; set; } = new();
    public bool IsCallRecordingAllowed { get; set; }
}
