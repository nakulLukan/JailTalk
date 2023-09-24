using JailTalk.Shared;
using JailTalk.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace JailTalk.Application.Dto.Prison;

public class NewPrisonerDto
{
    [Required]
    [MaxLength(10)]
    [RegularExpression(RegularExpressionPatternConstant.IntegerOnly)]
    public string Pid { get; set; }

    [Required]
    public int? JailId { get; set; }

    [Required]
    [MaxLength(50)]
    [RegularExpression(RegularExpressionPatternConstant.PersonName)]
    public string FirstName { get; set; }
    [MaxLength(50)]
    [RegularExpression(RegularExpressionPatternConstant.PersonName)]
    public string MiddleName { get; set; }

    [Required]
    public Gender Gender { get; set; }

    [MaxLength(50)]
    [RegularExpression(RegularExpressionPatternConstant.PersonName)]
    public string LastName { get; set; }
}
