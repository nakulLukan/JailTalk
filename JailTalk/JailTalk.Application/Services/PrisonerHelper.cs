using JailTalk.Shared.Utilities;

namespace JailTalk.Application.Services;

public class PrisonerHelper
{
    public static bool IsUnlimitedCallPriviledgeEnabled(DateTimeOffset? allowedTill)
        => allowedTill.HasValue ? allowedTill.Value > AppDateTime.UtcNow : false;
}
