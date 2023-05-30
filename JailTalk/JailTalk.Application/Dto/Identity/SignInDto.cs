using System.ComponentModel.DataAnnotations;

namespace JailTalk.Application.Dto.Identity;

public class SignInDto
{
    [Required]
    public string? Username { get; set; }

    [Required]
    public string? Password { get; set; }
}
