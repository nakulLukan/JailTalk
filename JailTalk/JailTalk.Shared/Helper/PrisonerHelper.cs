namespace JailTalk.Shared.Helper;

public class PrisonerHelper
{
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
}
