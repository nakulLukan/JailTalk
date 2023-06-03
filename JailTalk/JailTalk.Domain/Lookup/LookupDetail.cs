namespace JailTalk.Domain.Lookup;

public class LookupDetail
{
    public int Id { get; set; }
    public int LookupMasterId { get; set; }
    public string InternalName { get; set; }
    public bool IsActive { get; set; }
    public string Value { get; set; }
    public int? Order { get; set; }

    public LookupMaster LookupMaster { get; set; }
}
