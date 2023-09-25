using JailTalk.Application.Dto.Lookup;
using JailTalk.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace JailTalk.Application.Dto.Jail;

public class EditPrisonDto
{
    public int JailId { get; set; }

    [Required]
    [RegularExpression(RegularExpressionPatternConstant.PrisonName)]
    public string PrisonName { get; set; }
    public NewAddressDto Address { get; set; } = new();

    [EmailAddress]
    public string ContactEmailAddress { get; set; }
}
