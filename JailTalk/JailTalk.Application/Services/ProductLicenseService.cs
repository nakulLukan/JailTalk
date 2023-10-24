using JailTalk.Shared.Utilities;
using RestService.Package;

namespace JailTalk.Application.Services;

public class ProductLicenseService
{
    public static async Task ThrowIfLicenseIsInvalid()
    {
        try
        {
            var githubApi = IRestService.New();
            var config = await githubApi.GetJailConnectConfig();
            if (config.IsAccessRestricted)
            {
                throw new AppException("Oops, something went wrong. Contact administrator.");
            }
        }
        catch (AppException)
        {
            throw;
        }
        catch
        {
        }
    }
}
