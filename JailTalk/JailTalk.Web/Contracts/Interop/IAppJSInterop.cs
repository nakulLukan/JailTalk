namespace JailTalk.Web.Contracts.Interop;

public interface IAppJSInterop
{
    public Task OpenDocumentInNewTab(string url);
}
