namespace JailTalk.Application.Contracts.Http;

public interface IAppRequestContext
{
    Task<string> GetUserId();
}
