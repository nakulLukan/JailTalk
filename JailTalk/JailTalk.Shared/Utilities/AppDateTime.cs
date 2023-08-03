namespace JailTalk.Shared.Utilities;

public class AppDateTime
{
    public static DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    public static DateTimeOffset UtcNowAtStartOfTheDay => DateTimeOffset.UtcNow.Date.ToUniversalTime();
}
