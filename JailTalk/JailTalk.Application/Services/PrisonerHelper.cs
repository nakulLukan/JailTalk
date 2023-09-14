using JailTalk.Shared.Utilities;
using System.Globalization;

namespace JailTalk.Application.Services;

public class PrisonerHelper
{
    public static bool IsUnlimitedCallPriviledgeEnabled(DateTimeOffset? allowedTill)
        => allowedTill.HasValue ? allowedTill.Value > AppDateTime.UtcNow : false;


    public static bool IsPrisonerBlocked(bool isPermanentlyBlocked, DateTimeOffset? punishmentEndsOn)
    {
        return isPermanentlyBlocked || punishmentEndsOn.HasValue && punishmentEndsOn.Value > AppDateTime.UtcNow;
    }

    /// <summary>
    /// Converts given <paramref name="isActive"/> and <paramref name="isBlocked"/> to text representation
    /// </summary>
    /// <param name="isActive"></param>
    /// <param name="isBlocked"></param>
    /// <returns></returns>
    public static string ConvertContactStateAsText(bool isActive, bool isBlocked)
    {
        return isActive switch
        {
            true when !isBlocked => "Enabled",
            true when isBlocked => "Enabled, Blocked",
            false when !isBlocked => "Disabled",
            false when isBlocked => "Disabled, Blocked",
            _ => string.Empty
        };
    }

    /// <summary>
    /// Get prisoners base attachment path
    /// </summary>
    /// <param name="pid">Prisoners PID value</param>
    /// <returns></returns>
    public static string GetPrisonerAttachmentBasePath(string pid)
    {
        return $"prisoners/{pid}";
    }
}
