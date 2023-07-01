using JailTalk.Shared;

namespace JailTalk.Application.Contracts.Data;

public interface IApplicationSettingsProvider
{
    /// <summary>
    /// Unit: Minutes
    /// </summary>
    /// <param name="gender"></param>
    /// <returns></returns>
    Task<int> GetMaxAllowedCallDuration(Gender gender);
}
