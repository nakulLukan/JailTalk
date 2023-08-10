﻿namespace JailTalk.Shared.Utilities;

public class AppDateTime
{
    public static DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    public static DateTimeOffset UtcNowAtStartOfTheDay => DateTimeOffset.UtcNow.Date.ToUniversalTime();
    public static DateTimeOffset UtcDateBeforeNDays(int numberOfDays) => UtcNowAtStartOfTheDay.AddDays(-numberOfDays).ToUniversalTime();
    public static DateTimeOffset TillEndOfDay => UtcNowAtStartOfTheDay.AddDays(1);
}
