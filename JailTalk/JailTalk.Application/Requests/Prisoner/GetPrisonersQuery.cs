using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Prison;
using JailTalk.Shared.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prisoner;

public class GetPrisonersQuery : IRequest<List<PrisonerListDto>>
{
    public int JailId { get; set; }
}

public class GetPrisonersQueryHandler : IRequestHandler<GetPrisonersQuery, List<PrisonerListDto>>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;

    public GetPrisonersQueryHandler(IAppDbContext dbContext, IAppRequestContext requestContext)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
    }

    public async Task<List<PrisonerListDto>> Handle(GetPrisonersQuery request, CancellationToken cancellationToken)
    {
        var associatedPrison = _requestContext.GetAssociatedPrisonId();

        var prisoners = await _dbContext.Prisoners
            .WhereInPrison(x => x.JailId, associatedPrison)
            .Where(x => x.JailId.HasValue)
            .OrderBy(x => x.FirstName)
            .Select(x => new PrisonerListDto
            {
                FullName = x.FullName,
                Pid = x.Pid,
                PrisonCode = x.Jail.Code,
                Id = x.Id
            })
            .ToListAsync(cancellationToken);

        int index = 1;
        prisoners.ForEach(x => x.Serial = index++);
        return prisoners;
    }
}

