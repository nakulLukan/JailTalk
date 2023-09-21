namespace JailTalk.Application.Dto.Prison;

public class ShowContactsDto
{
    public string CountryCode { get; set; }
    public string PhoneNumber { get; set; }
    public string ContactRelationName { get; set; }
    public string RelativeName { get; set; }

    /// <summary>
    /// Flag to indicate whether phone call should be recorded when making a call to this contact.
    /// </summary>
    public bool IsCallRecordingEnabled { get; set; }
    public long Id { get; set; }
}
