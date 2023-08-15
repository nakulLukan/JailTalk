using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Prison;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prison;

public class AccountBalanceQuery : IRequest<ResponseDto<AccountBalanceDto>>
{
    public Guid PrisonerId { get; set; }
}

public class AccountBalanceQueryHandler : IRequestHandler<AccountBalanceQuery, ResponseDto<AccountBalanceDto>>
{
    readonly IAppDbContext _dbContext;
    readonly IApplicationSettingsProvider _settingsProvider;
    readonly IAppRequestContext _requestContext;

    public AccountBalanceQueryHandler(IAppDbContext dbContext, IApplicationSettingsProvider settingsProvider, IAppRequestContext requestContext)
    {
        _dbContext = dbContext;
        _settingsProvider = settingsProvider;
        _requestContext = requestContext;
    }

    public async Task<ResponseDto<AccountBalanceDto>> Handle(AccountBalanceQuery request, CancellationToken cancellationToken)
    {
        var prisonId = _requestContext.GetAssociatedPrisonId();
        var chargePerMinute = await _settingsProvider.GetCallPriceChargedPerMinute();
        var accountInfo = await _dbContext.PhoneBalances
            .WhereInPrison(x => x.Prisoner.JailId, prisonId)
            .Where(x => x.PrisonerId == request.PrisonerId)
            .Select(x => new AccountBalanceDto
            {
                PrisonerId = request.PrisonerId,
                AccountBalanceAmount = x.Balance,
                TalkTimeLeft = ((float?)(x.Balance / chargePerMinute)).ToHoursMinutesSeconds(),
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (accountInfo is null)
        {
            accountInfo = new()
            {
                AccountBalanceAmount = 0,
                PrisonerId = request.PrisonerId,
                TalkTimeLeft = "-"
            };
        }

        return new(accountInfo);
    }
}
