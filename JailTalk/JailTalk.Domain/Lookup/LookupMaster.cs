namespace JailTalk.Domain.Lookup;

public class LookupMaster
{
    public int Id { get; set; }
    public string InternalName { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }

    public List<LookupDetail> LookupDetails { get; set; }
}
