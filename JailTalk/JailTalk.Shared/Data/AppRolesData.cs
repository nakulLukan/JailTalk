using System.Collections.ObjectModel;

namespace JailTalk.Shared.Data;

public static class AppRolesData
{
    public static readonly ReadOnlyCollection<AppRoleDataDto> Roles = new List<AppRoleDataDto>()
    {
        new AppRoleDataDto()
        {
            RoleLevel = 0,
            RoleName = "super-admin",
            DisplayName = "Super Admin",
            Description = "Does have full access to the application.",
        },
        new AppRoleDataDto()
        {
            RoleLevel = 1,
            RoleName = "supervisor",
            DisplayName = "Supervisor",
            Description = "Does have full access to the application. The data is restricted to the users associated prison. The user cannot view data of other prisons."
        },
        new AppRoleDataDto()
        {
            RoleLevel = 2,
            RoleName = "subordinate",
            DisplayName = "Super Admin",
            Description = "Does have only limited access to the application. The data is restricted to the users associated prison."
        }
    }.AsReadOnly();
}

public class AppRoleDataDto
{
    public string RoleName { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }

    /// <summary>
    /// This can be used to show or hide roles based on current users role.
    /// For example: A least privileged user role may not able to see roles which have high privilege. 
    /// </summary>
    public int RoleLevel { get; set; }
}
