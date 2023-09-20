using JailTalk.Shared.Constants;
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
    [Required]
    [RegularExpression(RegularExpressionPatternConstant.Pincode, ErrorMessage = "Wrong format. Enter value in the following format 'xxxxxx'")]
    public string PinCode { get; set; }

    public int? StateId { get; set; }
    public int? CountryId { get; set; }
}
