using Humanizer;

namespace JailTalk.Shared.Extensions;
public static class NumberExtension
{
    public static string ToCurrency(this float value)
    {
        return value.ToString("C", System.Globalization.CultureInfo.CurrentCulture);
    }

    public static string ToCurrency(this float? value)
    {
        if (value.HasValue)
        {
            return value.Value.ToString("C", System.Globalization.CultureInfo.CurrentUICulture);
        }

        return String.Empty;
    }

    public static string ToCurrencyAsAscii(this float value)
    {
        return value.ToString("0.00");
    }

    public static string ToCurrencyAsAscii(this float? value)
    {
        if (value.HasValue)
        {
            return ToCurrencyAsAscii(value.Value);
        }

        return String.Empty;
    }

    public static string ToHoursMinutesSeconds(this float? value)
    {
        if (value.HasValue)
        {
            return TimeSpan.FromMinutes(value.Value)
                .Humanize(3,
                maxUnit: Humanizer.Localisation.TimeUnit.Hour,
                minUnit: Humanizer.Localisation.TimeUnit.Second);
        }

        return String.Empty;
    }
}