﻿namespace JailTalk.Application.Dto.Device;

public class DeviceListDto
{
    public Guid DeviceId { get; set; }
    public int Serial { get; set; }
    public string Code { get; set; }
    public string DeviceSecret { get; set; }
    public bool IsActive { get; set; }
    public string Prison { get; set; }
}
