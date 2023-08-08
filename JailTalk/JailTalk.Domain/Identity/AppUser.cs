using JailTalk.Domain.Prison;
using Microsoft.AspNetCore.Identity;

namespace JailTalk.Domain.Identity;

public class AppUser : IdentityUser
{
    /// <summary>
    /// Full name of the user
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Flag to indicate whether this user was seeded. All users with this field as true will get deleted if not mentioned in the
    /// appsettings.json
    /// </summary>
    public bool IsSystemGeneratedUser { get; set; }

    /// <summary>
    /// The prison to which this user is associated to administrate.
    /// If value is null then the user has access to all prisons in the system
    /// </summary>
    public int? PrisonId { get; set; }

    public Jail Prison { get; set; }
}