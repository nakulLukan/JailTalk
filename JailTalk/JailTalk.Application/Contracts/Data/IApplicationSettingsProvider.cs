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

    /// <summary>
    /// Amount in rupees charged per minute phone call.
    /// </summary>
    /// <returns></returns>
    Task<float> GetCallPriceChargedPerMinute();

    /// <summary>
    /// The number of allowed contacts a prisoner can choose to select from the device.
    /// </summary>
    /// <returns></returns>
    Task<int> GetMaxContactNumbersInActiveState();

    /// <summary>
    /// Flag to indicate whether access to call recordings is enabled.
    /// </summary>
    /// <returns></returns>
    Task<bool> GetAllowAccessToCallRecording();
}
