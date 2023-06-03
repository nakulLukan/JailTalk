namespace JailTalk.Domain.Lookup;
public class AddressBook
{
    public long Id { get; set; }
    public string HouseName { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public int? StateId { get; set; }
    public int? CountryId { get; set; }

    public LookupDetail State { get; set; }
    public LookupDetail Country { get; set; }
}
