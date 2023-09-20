namespace JailTalk.Application.Dto.Prison;

public class PrisonerContactDetailListDto
{
    public long Id { get; set; }
    public int Serial { get; set; }
    public string ContactNumber { get; set; }
    public string Relationship { get; set; }
    public string Name { get; set; }
    public string RelativeAddress { get; set; }

    /// <summary>
    /// If proof is uploaded then the proof type value is shown else 'Not Uploaded' text will be shown.
    /// </summary>
    public string ProofType { get; set; }

    /// <summary>
    /// Text representation of whether contact is blocked or active
    /// </summary>
    public string Status { get; set; }
    public bool IsBlocked { get; set; }
    public bool IsActive { get; set; }
}
