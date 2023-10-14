using Fluxor;

namespace JailTalk.Web.Store.Http;

public class HttpCallRequestFeature
{
    public record WaitForHttpResponseAction();
    public record HttpResponseRecievedAction();

    public static class Reducers
    {
        [ReducerMethod]
        public static HttpCallRequestState ReduceWaitForHttpResponseAction(HttpCallRequestState state, WaitForHttpResponseAction action)
        {
            return new HttpCallRequestState
            {
                NumberOfSubscribers = state.NumberOfSubscribers + 1,
            };
        }

        [ReducerMethod]
        public static HttpCallRequestState ReduceHttpResponseRecievedAction(HttpCallRequestState state, HttpResponseRecievedAction action)
        {
            return new HttpCallRequestState
            {
                NumberOfSubscribers = state.NumberOfSubscribers - 1,
            };
        }
    }
}