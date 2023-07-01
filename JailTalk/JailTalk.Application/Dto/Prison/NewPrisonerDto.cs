using JailTalk.Application.Dto.Lookup;
using JailTalk.Shared;
using System.ComponentModel.DataAnnotations;

namespace JailTalk.Application.Dto.Prison;

public class NewPrisonerDto
{
    [Required]
    [MaxLength(50)]
    public string Pid { get; set; }

    [Required]
    public int? JailId { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    [MaxLength(50)]
    public string MiddleName { get; set; }

    [Required]
    public Gender Gender { get; set; }

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }
    public NewAddressDto Address { get; set; } = new NewAddressDto();
}
