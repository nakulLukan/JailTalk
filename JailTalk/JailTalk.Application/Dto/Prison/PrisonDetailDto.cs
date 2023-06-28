using JailTalk.Application.Dto.Lookup;

namespace JailTalk.Application.Dto.Prison;

public class PrisonerDetailDto
{
    public Guid Id { get; set; }
    public string Pid { get; set; }
    public string JailName { get; set; }
    public string JailCode { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string AddressAsText { get; set; }
    public AddressDetailDto Address { get; set; } = new AddressDetailDto();
}
