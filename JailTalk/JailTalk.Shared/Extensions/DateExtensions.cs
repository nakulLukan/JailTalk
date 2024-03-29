﻿using Humanizer;

namespace JailTalk.Shared.Extensions;

public static class DateExtensions
{
    private static readonly TimeZoneInfo _timeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

    public static string ToDateString(this DateTimeOffset? dateTime)
    {
        return dateTime.HasValue ? dateTime.Value.ToString("yyyy-MM-dd") : string.Empty;
    }

    public static string ToFileTimeString(this DateTimeOffset dateTime)
    {
        return dateTime.ToString("yyyyMMddHHmmss");
    }

    public static string ToLocalDateTimeString(this DateTimeOffset dateTime)
    {
        return dateTime.ToOffset(_timeZone.BaseUtcOffset).ToString("yyyy-MM-dd HH:mm:ss");
    }

    /// <summary>
    /// Converted given number of minutes into string in following format <b>HH hours, MM minutes, SS seconds</b>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToHoursMinutesSeconds(this TimeSpan? value)
    {
        if (value.HasValue)
        {
            return value.Value
                .Humanize(3,
                maxUnit: Humanizer.Localisation.TimeUnit.Hour,
                minUnit: Humanizer.Localisation.TimeUnit.Second);
        }

        return String.Empty;
    }

    public static string ToTimeString(this TimeSpan timeSpan)
    {
        return DateTime.Today.Add(timeSpan).ToString("hh\\:mm tt");
    }
}
