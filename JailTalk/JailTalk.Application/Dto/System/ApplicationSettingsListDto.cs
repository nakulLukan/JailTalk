namespace JailTalk.Application.Dto.System;

public class ApplicationSettingsListDto
{
    public string ApplicationSettingId { get; set; }
    public string Value { get; set; }
    public string Description { get; set; }
    public bool IsReadonly { get; set; }
    public string InputValidationRegexPattern { get; set; }
    public string LastUpdatedOn { get; set; }
}
