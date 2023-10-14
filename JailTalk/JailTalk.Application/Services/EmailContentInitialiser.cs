using JailTalk.Shared.Extensions;

namespace JailTalk.Application.Services;

public static class EmailContentInitialiser
{
    public static (string TemplateName, IDictionary<string, string> BodyParams) RechargeRequestBuilder(string rechargeRequestDetails)
    {
        IDictionary<string, string> bodyParams = new Dictionary<string, string>
        {
            { "@RechargeRequest", rechargeRequestDetails }
        };
        return ("recharge-request", bodyParams);
    }
}
