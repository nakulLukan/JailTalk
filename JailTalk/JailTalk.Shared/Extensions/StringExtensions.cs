namespace JailTalk.Shared.Extensions;

public static class StringExtensions
{
    public static string Normalized(this string value)
    {
        if (value == null) return null;
        return value.ToUpperInvariant();
    }

    public static string Mask(this string value, int unmaskFirst, int unmaskLast)
    {
        char[] maskedValue = value.ToCharArray();
        unmaskLast = value.Length - unmaskLast;
        for (int i = 0; i < value.Length; i++)
        {
            if (i < unmaskFirst || i >= unmaskLast)
            {
                maskedValue[i] = value[i];
            }
            else
            {
                maskedValue[i] = '*';
            }
        }

        return string.Join(string.Empty, maskedValue);
    }
}
