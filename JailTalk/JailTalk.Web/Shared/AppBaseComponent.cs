using Microsoft.AspNetCore.Components;

namespace JailTalk.Web.Shared;

public class AppBaseComponent : ComponentBase
{
    protected void ChangeToLoadingState(ref bool isLoading)
    {
        isLoading = true;
        StateHasChanged();
    }

    protected void ChangeToLoadedState(ref bool isLoading)
    {
        isLoading = false;
        InvokeAsync(StateHasChanged);
    }
}
