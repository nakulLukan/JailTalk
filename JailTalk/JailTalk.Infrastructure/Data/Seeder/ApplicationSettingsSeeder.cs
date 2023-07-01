using JailTalk.Domain.System;
using JailTalk.Shared;
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
                Description = "Max number of contacts to show to a prisoner"
            },
            new()
            {
                Id = ApplicationSettings.MaxAllowedCallDurationMale,
                Value = 15.ToString(),
                Description = "Max number of minutes a male prisoner can make a call."
            },
            new()
            {
                Id = ApplicationSettings.MaxAllowedCallDurationFemale,
                Value = 20.ToString(),
                Description = "Max number of minutes a female prisoner can make a call."
            },
            new()
            {
                Id = ApplicationSettings.MaxAllowedCallDurationOthers,
                Value = 15.ToString(),
                Description = "Max number of minutes a LGBTQ+ prisoner can make a call."
            }
        };

        foreach (var seed in appsettings)
        {
            modelBuilder.Entity<ApplicationSetting>().HasData(seed);
        }
    }
}
