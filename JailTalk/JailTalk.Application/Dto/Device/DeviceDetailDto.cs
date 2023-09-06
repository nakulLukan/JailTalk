
namespace JailTalk.Application.Dto.Device;

public class DeviceDetailDto
{
    public Guid DeviceId { get; set; }

    public string Code { get; set; } = string.Empty;

    public int? PrisonId { get; set; }

    public string MacAddress { get; set; } = string.Empty;

    public string DeviceSecret { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
