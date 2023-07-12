namespace JailTalk.Domain.Identity;
public class AppFaceEncoding
{
    public int Id { get; set; }
    public double[] Encoding { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset? LastModifiedOn { get; set; }
    public string LastModifiedBy { get; set; }
    public string EncodingName { get; set; }
}
