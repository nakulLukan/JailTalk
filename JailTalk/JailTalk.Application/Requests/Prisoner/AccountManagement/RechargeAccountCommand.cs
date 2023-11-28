using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Domain.Prison;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prisoner.AccountManagement;

public class RechargeAccountCommand : IRequest<ResponseDto<float>>
{
    public Guid PrisonerId { get; set; }
    public float RechargeAmount { get; set; }
}

public class RechargeAccountCommandHandler : IRequestHandler<RechargeAccountCommand, ResponseDto<float>>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _appRequestContext;

    public RechargeAccountCommandHandler(IAppDbContext dbContext, IAppRequestContext appRequestContext)
    {
        _dbContext = dbContext;
        _appRequestContext = appRequestContext;
    }

    public async Task<ResponseDto<float>> Handle(RechargeAccountCommand request, CancellationToken cancellationToken)
    {
        float initialAmount = 0;
        var jail = await _dbContext.Prisoners
            .Where(x => x.Id == request.PrisonerId)
            .Select(x => new
            {
                AccountBalance = x.Jail.AccountBalance,
                x.JailId
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (jail.AccountBalance == null || (jail.AccountBalance.BalanceAmount < request.RechargeAmount))
        {
            throw new AppException("Jail account has insufficient balance. Please recharge the prison account balance.");
        }

        var accountBalanceInfo = await _dbContext.PhoneBalances.AsTracking()
            .Where(x => x.PrisonerId == request.PrisonerId)
            .FirstOrDefaultAsync(cancellationToken);
        if (accountBalanceInfo == null)
        {
            accountBalanceInfo = new PhoneBalance
            {
                PrisonerId = request.PrisonerId,
                LastUpdatedOn = AppDateTime.UtcNow,
                Balance = request.RechargeAmount,
            };

            _dbContext.PhoneBalances.Add(accountBalanceInfo);
        }
        else
        {
            initialAmount = accountBalanceInfo.Balance;
            accountBalanceInfo.Balance += request.RechargeAmount;
        }

        var balanceHistoryEntity = new PhoneBalanceHistory
        {
            AmountDifference = request.RechargeAmount,
            CallRequestId = null,
            CreatedOn = AppDateTime.UtcNow,
            NetAmount = accountBalanceInfo.Balance,
            PrisonerId = request.PrisonerId,
            Reason = Shared.PhoneBalanceReason.Recharge,
            RechargedByUserId = await _appRequestContext.GetUserId()
        };

        _dbContext.PhoneBalanceHistory.Add(balanceHistoryEntity);
        await _dbContext.JailAccountBalance
            .Where(x=>x.JailId == jail.JailId)
            .ExecuteUpdateAsync((x) => x.SetProperty(
            x => x.BalanceAmount,
            x => x.BalanceAmount - request.RechargeAmount));
        await _dbContext.SaveAsync(cancellationToken);
        return new ResponseDto<float>(accountBalanceInfo.Balance);
    }
}

