using System.ComponentModel.DataAnnotations;

namespace JailTalk.Application.Dto.Identity;

public class SignInDto
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
}
