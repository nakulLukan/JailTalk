using System.ComponentModel.DataAnnotations;

namespace JailTalk.Application.Requests.Lookup;

public class NewAddressDto
{
    [Required]
    [MaxLength(50)]
    public string HouseName { get; set; }
    [MaxLength(50)]
    public string Street { get; set; }

    [Required]
    [MaxLength(50)]
    public string City { get; set; }

    [Required]
    [MaxLength(10)]
    public string PinCode { get; set; }
    public int? StateId { get; set; }
    public int? CountryId { get; set; }
}
