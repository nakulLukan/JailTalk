using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Dto.Prison;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prison;

public class GetPrisonersQuery : IRequest<List<PrisonerListDto>>
{
    public int? JailId { get; set; }
}

public class GetPrisonersQueryHandler : IRequestHandler<GetPrisonersQuery, List<PrisonerListDto>>
{
    readonly IAppDbContext _dbContext;

    public GetPrisonersQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<PrisonerListDto>> Handle(GetPrisonersQuery request, CancellationToken cancellationToken)
    {
        var prisoners = await _dbContext.Prisoners
            .Where(x => (request.JailId.HasValue && x.JailId == request.JailId)
                || (!request.JailId.HasValue))
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

