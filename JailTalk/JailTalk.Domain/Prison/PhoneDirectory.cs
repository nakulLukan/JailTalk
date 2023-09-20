using JailTalk.Domain.Lookup;
using JailTalk.Domain.System;

namespace JailTalk.Domain.Prison;
public class PhoneDirectory : DomainBase
{
    public long Id { get; set; }
    public Guid PrisonerId { get; set; }
    public string PhoneNumber { get; set; }
    public string CountryCode { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public bool IsBlocked { get; set; }
    public DateTimeOffset? LastBlockedOn { get; set; }
    public int RelativeTypeId { get; set; }
    public long RelativeAddressId { get; set; }
    public int? IdProofTypeId { get; set; }
    public string IdProofValue { get; set; }
    public int? IdProofAttachmentId { get; set; }

    public Prisoner Prisoner { get; set; }
    public LookupDetail RelativeType { get; set; }
    public LookupDetail IdProofType { get; set; }
    public AddressBook RelativeAddress { get; set; }
    public Attachment IdProofAttachment { get; set; }
    public List<CallHistory> CallHistory { get; set; }
}
