namespace JailTalk.Web.Impl.Identity;

public interface IAppAuthenticator
{
    public bool CheckHasPermission(string roles);
}
