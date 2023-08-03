using JailTalk.Domain.System;
using JailTalk.Shared;
using JailTalk.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Infrastructure.Data.Seeder;

public class ApplicationSettingsSeeder
{
    public static void SeedData(ModelBuilder modelBuilder)
    {
        List<ApplicationSetting> appsettings = new()
        {
            new()
            {
                Id = ApplicationSettings.MaxAllowedActiveContacts,
                Value = 3.ToString(),
                Description = "Max number of contacts to show to a prisoner",
                RegexValidation = RegularExpressionPatternConstant.IntegerOnly
            },
            new()
            {
                Id = ApplicationSettings.MaxAllowedCallDurationMale,
                Value = 30.ToString(),
                Description = "Max number of minutes a male prisoner can make a call a day.",
                RegexValidation = RegularExpressionPatternConstant.IntegerOnly
            },
            new()
            {
                Id = ApplicationSettings.MaxAllowedCallDurationFemale,
                Value = 30.ToString(),
                Description = "Max number of minutes a female prisoner can make a call a day.",
                RegexValidation = RegularExpressionPatternConstant.IntegerOnly
            },
            new()
            {
                Id = ApplicationSettings.MaxAllowedCallDurationOthers,
                Value = 30.ToString(),
                Description = "Max number of minutes a LGBTQ+ prisoner can make a call a day.",
                RegexValidation = RegularExpressionPatternConstant.IntegerOnly
            },
            new()
            {
                Id = ApplicationSettings.CallPricePerMinute,
                Value = (0.5).ToString(),
                Description = "Amount charged for 1 minute call in rupees.",
                RegexValidation = RegularExpressionPatternConstant.FloatOnly
            },
            new()
            {
                Id = ApplicationSettings.MaxAllowedCallTimeInRupeesPerMonth,
                Value = (450).ToString(),
                Description = "Maximum allowed call time per month in rupees. The prisoner cannot make any call of the total talktime (in minutes) amount in a month crosses this value.",
                RegexValidation = RegularExpressionPatternConstant.IntegerOnly
            }
        };

        foreach (var seed in appsettings)
        {
            modelBuilder.Entity<ApplicationSetting>().HasData(seed);
        }
    }
}
