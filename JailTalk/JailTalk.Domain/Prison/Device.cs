namespace JailTalk.Domain.Prison;
public class Device
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public Guid DeviceSecretIdentifier { get; set; }
    public int JailId { get; set; }
    public string MacAddress { get; set; }
    public bool IsActive { get; set; }
    public int FailedLoginAttempts { get; set; }
    public DateTimeOffset? LastLoggedOn { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }

    public Jail Jail { get; set; }
}
