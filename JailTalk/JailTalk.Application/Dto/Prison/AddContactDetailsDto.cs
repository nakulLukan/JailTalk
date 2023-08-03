using JailTalk.Application.Dto.Lookup;
using JailTalk.Shared;
using JailTalk.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace JailTalk.Application.Dto.Prison;
public class AddContactDetailsDto
{   
    public Guid PrisonerId { get; set; }

    [Required]
    [MaxLength(5)]
    [RegularExpression(RegularExpressionPatternConstant.CountryCode)]
    public string CountryCode { get; set; }

    [MaxLength(15)]
    [RegularExpression(RegularExpressionPatternConstant.PhoneNumber)]
    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    public int? RelationshipId { get; set; }
    public NewAddressDto RelativeAddress { get; set; } = new();
    public bool IsEnabled { get; set; } = true;
}
