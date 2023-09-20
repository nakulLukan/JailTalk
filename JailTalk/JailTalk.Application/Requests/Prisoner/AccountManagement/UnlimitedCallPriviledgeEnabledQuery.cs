using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Services;
using JailTalk.Shared;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prisoner.AccountManagement;

public class UnlimitedCallPriviledgeEnabledQuery : IRequest<ResponseDto<bool>>
{
    public Guid PrisonerId { get; set; }
}
public class UnlimitedCallPriviledgeEnabledQueryHandler : IRequestHandler<UnlimitedCallPriviledgeEnabledQuery, ResponseDto<bool>>
{
    readonly IAppDbContext _dbContext;

    public UnlimitedCallPriviledgeEnabledQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResponseDto<bool>> Handle(UnlimitedCallPriviledgeEnabledQuery request, CancellationToken cancellationToken)
    {
        var prisoner = await _dbContext.Prisoners
            .Select(x => new
            {
                x.Id,
                x.PrisonerFunction.UnlimitedCallsEndsOn
            })
            .FirstOrDefaultAsync(x => x.Id == request.PrisonerId, cancellationToken) ?? throw new AppException(CommonExceptionMessages.PrisonerNotFound);
        return new(PrisonerHelper.IsUnlimitedCallPriviledgeEnabled(prisoner.UnlimitedCallsEndsOn));
    }
}

