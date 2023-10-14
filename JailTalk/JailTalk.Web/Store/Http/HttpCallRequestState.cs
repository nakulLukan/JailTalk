using Fluxor;

namespace JailTalk.Web.Store.Http;
[FeatureState]
public record HttpCallRequestState
{
    public int NumberOfSubscribers { get; set; }
}