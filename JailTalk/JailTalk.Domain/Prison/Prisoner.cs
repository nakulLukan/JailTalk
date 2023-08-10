using JailTalk.Domain.Lookup;
using JailTalk.Shared;

namespace JailTalk.Domain.Prison;
public class Prisoner : DomainBase
{
    public Guid Id { get; set; }
    public string Pid { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public long? AddressId { get; set; }
    public int? JailId { get; set; }
    public bool IsBlocked { get; set; }
    public bool IsActive { get; set; }

    /// <summary>
    /// This field can be used to let a prisoner to talk any number of times till the given date.
    /// Usually, this value should be 1 day.
    /// </summary>
    public DateTimeOffset? AllowUnlimitedCallsTill { get; set; }
    public Gender Gender { get; set; }

    public AddressBook Address { get; set; }
    public Jail Jail { get; set; }
    public PhoneBalance PhoneBalance { get; set; }
    public List<PhoneDirectory> PhoneDirectory { get; set; }
    public List<PrisonerFaceEncodingMapping> FaceEncodings { get; set; }
}
