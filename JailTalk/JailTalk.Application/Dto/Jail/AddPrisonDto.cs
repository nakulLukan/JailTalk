using JailTalk.Application.Dto.Lookup;
using JailTalk.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace JailTalk.Application.Dto.Jail;

public class AddPrisonDto
{
    [Required]
    [RegularExpression(RegularExpressionPatternConstant.PrisonCode)]
    public string PrisonCode { get; set; }

    [Required]
    [RegularExpression(RegularExpressionPatternConstant.PrisonName)]
    public string PrisonName { get; set; }
    public NewAddressDto Address { get; set; } = new();
}
