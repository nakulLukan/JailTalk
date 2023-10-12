using JailTalk.Shared.Extensions;

namespace JailTalk.Application.Services;

public static class EmailContentInitialiser
{
    public static (string TemplateName, IDictionary<string, string> BodyParams) RechargeRequestBuilder(string imgSrc)
    {
        IDictionary<string, string> bodyParams = new Dictionary<string, string>
        {
            { "@QRCodeSrc", imgSrc }
        };
        return ("recharge-request", bodyParams);
    }
}
