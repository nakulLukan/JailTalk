using System.ComponentModel.DataAnnotations;

namespace JailTalk.Application.Dto.Lookup;

public class NewAddressDto
{
    [MaxLength(50)]
    public string HouseName { get; set; }

    [MaxLength(50)]
    public string Street { get; set; }

    [MaxLength(50)]
    public string City { get; set; }

    [MaxLength(10)]
    public string PinCode { get; set; }
    public int? StateId { get; set; }
    public int? CountryId { get; set; }
}
