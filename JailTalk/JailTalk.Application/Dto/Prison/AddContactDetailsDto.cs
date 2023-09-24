using JailTalk.Application.Dto.Lookup;
using JailTalk.Shared.Constants;
using JailTalk.Shared.Models;
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
    [RegularExpression(RegularExpressionPatternConstant.PhoneNumber, ErrorMessage = "Phone number should have 10 numbers")]
    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    [MaxLength(50)]
    [RegularExpression(RegularExpressionPatternConstant.PersonName)]
    public string Name { get; set; }

    [Required]
    public int? RelationshipId { get; set; }
    public NewAddressDto RelativeAddress { get; set; } = new();
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Id proof of the contact
    /// </summary>
    public int? ContactProofTypeId { get; set; }

    /// <summary>
    /// Value of the id proof of the contact
    /// </summary>
    [RegularExpression(RegularExpressionPatternConstant.IdProof, ErrorMessage = "Specials characters should be limited. Only aphanumeric values with these ( ,-,/) special characters are allowed.")]
    [MaxLength(50)]
    public string ContactProofValue { get; set; }

    public BrowserFileDto ContactProofAttachment { get; set; }

    public bool IsCallRecordingAllowed { get; set; }
}
