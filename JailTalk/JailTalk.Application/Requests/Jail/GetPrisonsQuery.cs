using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Jail;
using JailTalk.Application.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Jail;

public class GetPrisonsQuery : IRequest<List<PrisonListDto>>
{
}
public class GetPrisonsQueryHandler : IRequestHandler<GetPrisonsQuery, List<PrisonListDto>>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;

    public GetPrisonsQueryHandler(IAppDbContext dbContext, IAppRequestContext requestContext)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
    }

    public async Task<List<PrisonListDto>> Handle(GetPrisonsQuery request, CancellationToken cancellationToken)
    {
        var prisons = await _dbContext.Jails.Include(x => x.Address)
                .Include(x => x.AccountBalance)
            .ToListAsync(cancellationToken);
        return prisons.Select(x => new PrisonListDto
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            AddressAsText = x.Address.AddressAsText(),
            IsSystemTurnedOff = x.IsSystemTurnedOff,
            AccountBalance = x.AccountBalance?.BalanceAmount ?? 0
        }).ToList();
    }
}

