using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Prison;
using JailTalk.Application.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prisoner.AccountManagement;

public class AccountBalanceQuery : IRequest<AccountBalanceDto>
{
    /// <summary>
    /// Value will be default if request is from mobile app.
    /// </summary>
    public Guid PrisonerId { get; set; }
}

public class AccountBalanceQueryHandler : IRequestHandler<AccountBalanceQuery, AccountBalanceDto>
{
    readonly IAppDbContext _dbContext;
    readonly IApplicationSettingsProvider _settingsProvider;
    readonly IAppRequestContext _requestContext;
    readonly IDeviceRequestContext _deviceRequestContext;

    public AccountBalanceQueryHandler(IAppDbContext dbContext, IApplicationSettingsProvider settingsProvider, IAppRequestContext requestContext, IDeviceRequestContext deviceRequestContext)
    {
        _dbContext = dbContext;
        _settingsProvider = settingsProvider;
        _requestContext = requestContext;
        _deviceRequestContext = deviceRequestContext;
    }

    public async Task<AccountBalanceDto> Handle(AccountBalanceQuery request, CancellationToken cancellationToken)
    {
        // If request is from mobile app then request context will be null.
        int? userJailId = null;
        if (_requestContext != null)
        {
            userJailId = _requestContext.GetAssociatedPrisonId();
        }

        // If request is from mobile app then prisoner id will be default in the request.
        // So update the value from the device token.
        if (request.PrisonerId == Guid.Empty)
        {
            request.PrisonerId = _deviceRequestContext.GetPrisonerId();
        }

        // Get account balance info.
        var accountInfo = await PrisonerAccountService.GetPrisonerAccountBalance((_dbContext, _settingsProvider),
            request.PrisonerId, userJailId, cancellationToken);
        accountInfo = await IfNullGetDefaultAccountInfo(request, accountInfo, cancellationToken);

        return accountInfo;
    }

    private async Task<AccountBalanceDto> IfNullGetDefaultAccountInfo(AccountBalanceQuery request, AccountBalanceDto accountInfo, CancellationToken cancellationToken)
    {
        if (accountInfo is null)
        {
            var prisonerBasicInfo = await _dbContext.Prisoners
                .Select(x => new
                {
                    x.Id,
                    x.Pid,
                    Name = PrisonerAccountService.GetPrisonerName(x)
                })
                .FirstOrDefaultAsync(x => x.Id == request.PrisonerId, cancellationToken);
            accountInfo = new()
            {
                AccountBalanceAmount = 0,
                PrisonerId = request.PrisonerId,
                TalkTimeLeft = "-",
                PrisonerName = prisonerBasicInfo.Name,
                Pid = prisonerBasicInfo.Pid,
            };
        }

        return accountInfo;
    }
}
