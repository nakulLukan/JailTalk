namespace JailTalk.Web.Impl.Events;

public class LoaderStateChangeNotifier
{
    public event Action StateChanged;

    public void NotifyStateChanged()
    {
        StateChanged?.Invoke();
    }
}
