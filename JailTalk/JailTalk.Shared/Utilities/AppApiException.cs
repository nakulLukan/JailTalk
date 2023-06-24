using System.Net;

namespace JailTalk.Shared.Utilities;
public class AppApiException : Exception
{
    public string ErrorMessage { get; }
    public HttpStatusCode StatusCode { get; }

    public AppApiException(HttpStatusCode statusCode, string errorMessage) : base(errorMessage)
    {
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
    }
}
