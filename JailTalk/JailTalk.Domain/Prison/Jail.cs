using JailTalk.Domain.Lookup;

namespace JailTalk.Domain.Prison;
public class Jail : DomainBase
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public long? AddressId { get; set; }
    public string ContactEmailAddress { get; set; }

    /// <summary>
    /// Flag to indicate whether all telephone devices in this jail is turned off and restricted to use by the prisoners.
    /// </summary>
    public bool IsSystemTurnedOff { get; set; }

    public AddressBook Address { get; set; }
}
