using JailTalk.Domain.Lookup;

namespace JailTalk.Domain.Prison;
public class PhoneDirectory : DomainBase
{
    public long Id { get; set; }
    public Guid PrisonerId { get; set; }
    public string PhoneNumber { get; set; }
    public string CountryCode { get; set; }
    public bool IsActive { get; set; }
    public bool IsBlocked { get; set; }
    public DateTimeOffset? LastBlockedOn { get; set; }
    public int RelativeTypeId { get; set; }
    public long RelativeAddressId { get; set; }

    public Prisoner Prisoner { get; set; }
    public LookupDetail RelativeType { get; set; }
    public AddressBook RelativeAddress { get; set; }
    public List<CallHistory> CallHistory { get; set; }
}
