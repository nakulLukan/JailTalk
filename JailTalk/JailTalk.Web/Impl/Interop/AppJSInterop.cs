using JailTalk.Web.Contracts.Interop;
using Microsoft.JSInterop;

namespace JailTalk.Web.Impl.Interop;

public class AppJSInterop : IAppJSInterop
{
    readonly IJSRuntime _jsRuntime;

    public AppJSInterop(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task OpenDocumentInNewTab(string url)
    {
        await _jsRuntime.InvokeAsync<object>("Helper.openInNewTab", url, string.Empty);
    }
}
