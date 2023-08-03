using JailTalk.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace JailTalk.Application.Dto.Identity;

public class AddUserAccountDto
{
    public string Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string FullName { get; set; }

    [Required]
    [MaxLength(255)]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; }

    [Required]
    [MaxLength(255)]
    public string Password { get; set; }

    [Required]
    [MaxLength(25)]
    [RegularExpression(RegularExpressionPatternConstant.RoleName)]
    public string RoleName { get; set; }

    public int? PrisonId { get; set; }
}
