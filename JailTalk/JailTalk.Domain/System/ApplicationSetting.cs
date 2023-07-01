using JailTalk.Shared;

namespace JailTalk.Domain.System;
public class ApplicationSetting
{
    public ApplicationSettings Id { get; set; }
    public string Value { get; set; }
    public string Description { get; set; }
}
