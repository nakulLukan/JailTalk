using JailTalk.Domain.Lookup;

namespace JailTalk.Domain.Prison;
public class Jail : DomainBase
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public long? AddressId { get; set; }

    public AddressBook Address { get; set; }
}
