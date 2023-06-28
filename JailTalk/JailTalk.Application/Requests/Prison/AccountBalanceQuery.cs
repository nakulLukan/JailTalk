using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Dto.Prison;
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

    public AccountBalanceQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResponseDto<AccountBalanceDto>> Handle(AccountBalanceQuery request, CancellationToken cancellationToken)
    {
        var accountInfo = await _dbContext.PhoneBalances
            .Where(x => x.PrisonerId == request.PrisonerId)
            .Select(x => new AccountBalanceDto
            {
                PrisonerId = request.PrisonerId,
                AccountBalanceAmount = x.Balance,
                TalkTimeLeft = x.Balance,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (accountInfo is null)
        {
            accountInfo = new()
            {
                AccountBalanceAmount = 0,
                PrisonerId = request.PrisonerId,
                TalkTimeLeft = 0
            };
        }

        return new(accountInfo);
    }
}
