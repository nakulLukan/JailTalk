using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Dto.Lookup;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Lookup;

public class LookupSelectQuery : IRequest<List<LookupSelectDto>>
{
    public string LookupMasterInternalName { get; set; }
}

public class LookupSelectQueryHandler : IRequestHandler<LookupSelectQuery, List<LookupSelectDto>>
{
    readonly IAppDbContext _dbContext;

    public LookupSelectQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<LookupSelectDto>> Handle(LookupSelectQuery request, CancellationToken cancellationToken)
    {
        var lookupValues = await _dbContext.LookupDetails
            .Where(x => x.LookupMaster.InternalName == request.LookupMasterInternalName)
            .Select(x => new LookupSelectDto
            {
                Id = x.Id,
                Value = x.Value
            })
            .ToListAsync(cancellationToken);

        return lookupValues;
    }
}
