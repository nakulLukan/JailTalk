using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Identity;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Identity;

public class UserAccountsQuery : IRequest<List<UserAccountListDto>>
{
}
public class UserAccountsQueryHandler : IRequestHandler<UserAccountsQuery, List<UserAccountListDto>>
{
    readonly IAppRequestContext _requestContext;
    readonly IAppDbContext _dbContext;

    public UserAccountsQueryHandler(IAppDbContext dbContext, IAppRequestContext requestContext)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
    }

    public async Task<List<UserAccountListDto>> Handle(UserAccountsQuery request, CancellationToken cancellationToken)
    {
        var prisonId = _requestContext.GetAssociatedPrisonId();
        var userId = await _requestContext.GetUserId();
        var users = await _dbContext.Users
            .WhereInPrison(x => x.PrisonId, prisonId)
            .Where(x => x.Id != userId)
            .Join(_dbContext.UserRoles,
            user => user.Id,
            userRole => userRole.UserId,
            (user, userRole) => new UserAccountListDto
            {
                AssociatedPrison = user.Prison.Code,
                Email = user.Email,
                FullName = user.FullName,
                Id = user.Id,
                IsAccountLocked = user.LockoutEnd.HasValue && user.LockoutEnd.Value > AppDateTime.UtcNow,
                UserName = user.UserName,
                RoleName = userRole.RoleId,
            })
            .OrderBy(x => x.FullName)
            .ToListAsync(cancellationToken);

        // Reset the role name by finding the actual value from db using role id
        var roles = await _dbContext.Roles.ToListAsync(cancellationToken);
        users.ForEach(x =>
        {
            x.RoleName = roles.FirstOrDefault(y => y.Id == x.RoleName)?.Name;
        });
        return users;
    }
}

