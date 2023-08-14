using Humanizer;
using JailTalk.Application.Dto.System;

namespace JailTalk.Application.Services;

public class ApplicationSettingsService
{
    public static ApplicationSettingsListDto GetApplicationSettingsDetails(Domain.System.ApplicationSetting applicationSettings)
    {
        return new ApplicationSettingsListDto
        {
            ApplicationSettingId = applicationSettings.Id.ToString(),
            Description = applicationSettings.Description,
            Value = applicationSettings.Value,
            IsReadonly = applicationSettings.IsReadonly,
            InputValidationRegexPattern = applicationSettings.RegexValidation,
            LastUpdatedOn = applicationSettings.LastUpdatedOn.Humanize(),
        };
    }
}
