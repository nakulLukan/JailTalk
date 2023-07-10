using JailTalk.Shared;
using System.ComponentModel.DataAnnotations;

namespace JailTalk.Application.Dto.Device;

public class NewDeviceDto
{
    [Required]
    [MaxLength(50)]
    [RegularExpression(RegularExpressionPattern.DeviceCode)]
    public string Code { get; set; }

    [Required]
    public int? PrisonId { get; set; }

    [Required]
    [MaxLength(50)]
    public string MacAddress { get; set; }

    public string DeviceSecret { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
