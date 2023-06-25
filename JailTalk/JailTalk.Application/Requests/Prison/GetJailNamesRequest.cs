using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Dto.Prison;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prison;

public class GetJailNamesRequest : IRequest<List<JailNameDto>>
{
}

public class GetJailNamesRequestHandler : IRequestHandler<GetJailNamesRequest, List<JailNameDto>>
{
    readonly IAppDbContext _dbContext;

    public GetJailNamesRequestHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<JailNameDto>> Handle(GetJailNamesRequest request, CancellationToken cancellationToken)
    {
        var jails = await _dbContext.Jails.OrderBy(x => x.Code).Select(x => new JailNameDto
        {
            Id = x.Id,
            Name = x.Name
        }).ToListAsync(cancellationToken);

        return jails;
    }
}

