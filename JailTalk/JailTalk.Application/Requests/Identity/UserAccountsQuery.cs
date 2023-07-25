using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Identity;
using MediatR;

namespace JailTalk.Application.Requests.Identity;

public class UserAccountsQuery : IRequest<List<UserAccountListDto>>
{
}
public class UserAccountsQueryHandler : IRequestHandler<UserAccountsQuery, List<UserAccountListDto>>
{
    readonly IAppRequestContext _requestContext;
    readonly IAppDbContext _dbContext;

    public UserAccountsQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<UserAccountListDto>> Handle(UserAccountsQuery request, CancellationToken cancellationToken)
    {
        return new();
    }
}

