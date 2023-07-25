namespace JailTalk.Application.Contracts.Http;

public interface IAppRequestContext
{
    /// <summary>
    /// Gets app user id
    /// </summary>
    /// <returns></returns>
    Task<string> GetUserId();

    /// <summary>
    /// Gets the prison id of the user. If value is 0 then the user is not associated to any prison.
    /// </summary>
    /// <returns></returns>
    int GetAssociatedPrisonId();
}
