namespace JailTalk.Shared.Extensions;

public static class StringExtensions
{
    public static string Normalize(this string value)
    {
        if (value == null) return null;
        return value.ToUpperInvariant();
    }
}
