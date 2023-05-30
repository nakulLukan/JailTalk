namespace JailTalk.Shared.Utilities;
public class AppException : Exception
{
    public string ErrorMessage { get; }

    public AppException(string errorMessage) : base(errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}
