using JailTalk.Domain.Prison;
using Microsoft.AspNetCore.Identity;

namespace JailTalk.Domain.Identity;

public class AppUser : IdentityUser
{
    /// <summary>
    /// The prison to which this user is associated to administrate.
    /// If value is null then the user has access to all prisons in the system
    /// </summary>
    public int? PrisonId { get; set; }

    public Jail Prison { get; set; }
}