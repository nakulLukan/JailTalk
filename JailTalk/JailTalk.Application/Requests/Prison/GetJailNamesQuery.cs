using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Prison;
using JailTalk.Shared.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prison;

public class GetJailNamesQuery : IRequest<List<JailNameDto>>
{
}

public class GetJailNamesQueryHandler : IRequestHandler<GetJailNamesQuery, List<JailNameDto>>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;

    public GetJailNamesQueryHandler(IAppDbContext dbContext, IAppRequestContext requestContext)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
    }

    public async Task<List<JailNameDto>> Handle(GetJailNamesQuery request, CancellationToken cancellationToken)
    {
        var prisonId = _requestContext.GetAssociatedPrisonId();
        var jails = await _dbContext.Jails
            .WhereInPrison(x => x.Id, prisonId)
            .OrderBy(x => x.Code).Select(x => new JailNameDto
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync(cancellationToken);

        return jails;
    }
}

