namespace JailTalk.Shared.Extensions;

public static class DateExtensions
{
    public static string ToDateString(this DateTimeOffset? dateTime)
    {
        return dateTime.HasValue ? dateTime.Value.ToString("yyyy-MM-dd") : string.Empty;
    }

    public static string ToFileTimeString(this DateTimeOffset dateTime)
    {
        return dateTime.ToString("yyyyMMddHHmmss");
    }
}
