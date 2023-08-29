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
    /// Text representation of whether contact is blocked or active
    /// </summary>
    public string Status { get; set; }
    public bool IsBlocked { get; set; }
    public bool IsActive { get; set; }
}
