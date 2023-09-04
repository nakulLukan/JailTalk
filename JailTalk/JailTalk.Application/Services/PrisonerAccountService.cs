using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Dto.Prison;
using JailTalk.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Services;

public static class PrisonerAccountService
{
    /// <summary>
    /// Get account balance of a given prisoner id
    /// </summary>
    /// <param name="services">All required services for this function</param>
    /// <param name="prisonerId">Id of the prisoner whose account balance has to be retrieved</param>
    /// <param name="userJailId">If value is available then the prisoner should be associated to this jail or else empty result will be provided</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    public static async Task<AccountBalanceDto> GetPrisonerAccountBalance(
        (IAppDbContext DbContext, IApplicationSettingsProvider SettingsProvider) services,
        Guid prisonerId,
        int? userJailId,
        CancellationToken cancellationToken)
    {
        var accountInfoQuery = services.DbContext.PhoneBalances.AsQueryable();
        var chargePerMinute = await services.SettingsProvider.GetCallPriceChargedPerMinute();
        if (userJailId.HasValue)
        {
            accountInfoQuery = accountInfoQuery.WhereInPrison(x => x.Prisoner.JailId, userJailId.Value);
        }

        var accountInfo = await accountInfoQuery.Where(x => x.PrisonerId == prisonerId)
            .Select(x => new AccountBalanceDto
            {
                PrisonerId = x.PrisonerId,
                AccountBalanceAmount = x.Balance,
                TalkTimeLeft = ((float?)(x.Balance / chargePerMinute)).ToHoursMinutesSeconds(),
            })
            .FirstOrDefaultAsync(cancellationToken);
        return accountInfo;
    }
}
