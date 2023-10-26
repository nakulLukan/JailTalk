namespace JailTalk.Application.Dto.Device;

public class DeviceListDto
{
    public Guid DeviceId { get; set; }
    public int Serial { get; set; }
    public string Code { get; set; }
    public string MacAddress { get; set; }
    public bool IsActive { get; set; }

    /// <summary>
    /// If all devices are disabled at the jail level.
    /// </summary>
    public bool IsDisabledAtJailLevel { get; set; }
    public string Prison { get; set; }
}
