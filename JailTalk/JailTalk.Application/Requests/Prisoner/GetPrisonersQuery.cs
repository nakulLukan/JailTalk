using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Prison;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prisoner;

public class GetPrisonersQuery : IRequest<PaginatedResponse<PrisonerListDto>>
{
    public int? JailId { get; set; }
    public string Pid { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}

public class GetPrisonersQueryHandler : IRequestHandler<GetPrisonersQuery, PaginatedResponse<PrisonerListDto>>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;

    public GetPrisonersQueryHandler(IAppDbContext dbContext, IAppRequestContext requestContext)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
    }

    public async Task<PaginatedResponse<PrisonerListDto>> Handle(GetPrisonersQuery request, CancellationToken cancellationToken)
    {
        var associatedPrison = _requestContext.GetAssociatedPrisonId();

        var prisonersQuery = _dbContext.Prisoners
            .WhereInPrison(x => x.JailId, associatedPrison)
            .Where(x => x.JailId.HasValue);
        if (!string.IsNullOrEmpty(request.Pid))
        {
            prisonersQuery = prisonersQuery.Where(x => x.Pid.Contains(request.Pid));
        }

        if (request.JailId.HasValue)
        {
            prisonersQuery = prisonersQuery.Where(x => x.JailId == request.JailId.Value);
        }
        var prisoners = await prisonersQuery.OrderBy(x => x.PidNumber)
        .Select(x => new PrisonerListDto
        {
            FullName = x.FullName,
            Pid = x.Pid,
            PrisonCode = x.Jail.Code,
            Id = x.Id,
            PrisonName = x.Jail.Name
        })
        .Skip(request.Skip.Value)
        .Take(request.Take.Value)
        .ToListAsync(cancellationToken);

        var prisonersCount = await prisonersQuery.CountAsync(cancellationToken);
        return new(prisoners, prisonersCount);
    }
}

