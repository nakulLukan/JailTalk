using JailTalk.Application.Dto.Lookup;
using JailTalk.Shared.Constants;
using JailTalk.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace JailTalk.Application.Dto.Prison;
public class EditContactDetailsDto
{
    public long ContactId { get; set; }
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
    [MaxLength(50)]
    [RegularExpression(RegularExpressionPatternConstant.PersonName)]
    public string Name { get; set; }

    [Required]
    public int? RelationshipId { get; set; }
    public NewAddressDto RelativeAddress { get; set; } = new();
    public bool IsCallRecordingAllowed { get; set; }
}
