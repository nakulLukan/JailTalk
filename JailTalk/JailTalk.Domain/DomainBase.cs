namespace JailTalk.Domain;
public class DomainBase
{
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
    public DateTimeOffset? CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
}
