using JailTalk.Shared;

namespace JailTalk.Domain.System;
public class ApplicationSetting
{
    public ApplicationSettings Id { get; set; }
    public string Value { get; set; }
    public string Description { get; set; }

    /// <summary>
    /// This expression will be used in the client side to validate the input.
    /// </summary>
    public string RegexValidation { get; set; }

    /// <summary>
    /// User can update the value from the application only if its value is true
    /// </summary>
    public bool IsReadonly { get; set; }
    public DateTimeOffset LastUpdatedOn { get; set; }
    public string LastUpdateBy { get; set; }
}
