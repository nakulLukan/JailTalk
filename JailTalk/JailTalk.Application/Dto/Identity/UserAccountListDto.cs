namespace JailTalk.Application.Dto.Identity;

public class UserAccountListDto
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string RoleName { get; set; }
    public string AssociatedPrison { get; set; }
    public bool IsAccountLocked { get; set; }
    public string UserName { get; set; }
}
