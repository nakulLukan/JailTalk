using Fluxor;

namespace JailTalk.Web.Store.Prisoner;
[FeatureState]
public record UnlimitedCallEnabledState
{
    public bool IsEnabled { get; init; }
}