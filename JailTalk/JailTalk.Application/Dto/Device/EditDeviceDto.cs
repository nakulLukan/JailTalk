using JailTalk.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace JailTalk.Application.Dto.Device;

public class EditDeviceDto
{
    public Guid DeviceId { get; set; }

    [Required]
    public int? PrisonId { get; set; }

    [Required]
    [MaxLength(50)]
    [RegularExpression(RegularExpressionPatternConstant.MacAddress, ErrorMessage = "Wrong format. Only alpha numeric values are allowed.")]
    public string MacAddress { get; set; }

    [Required]
    [RegularExpression(RegularExpressionPatternConstant.DeviceCode, ErrorMessage = "Wrong format. Enter value in the following format 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'")]
    [MaxLength(100)]
    public string DeviceSecret { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
