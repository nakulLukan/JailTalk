using Management.Models;
using Refit;

namespace Management.Contracts;

public interface IPrisonAccountManagementApiService
{
    [Post("/prison-account/recharge")]
    public Task<ApiResponseDto<string>> RechargeAccountAction(RechargeJailAccountRequest request);
}
