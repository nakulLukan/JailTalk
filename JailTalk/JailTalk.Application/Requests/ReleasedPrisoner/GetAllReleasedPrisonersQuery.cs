﻿using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Dto.ReleasedPrisoner;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.ReleasedPrisoner;

public class GetAllReleasedPrisonersQuery : IRequest<PaginatedResponse<ReleasedPrisonerListDto>>
{
    public string Pid { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}

public class GetAllReleasedPrisonersQueryHandler : IRequestHandler<GetAllReleasedPrisonersQuery, PaginatedResponse<ReleasedPrisonerListDto>>
{
    readonly IAppDbContext _dbContext;

    public GetAllReleasedPrisonersQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaginatedResponse<ReleasedPrisonerListDto>> Handle(GetAllReleasedPrisonersQuery request, CancellationToken cancellationToken)
    {
        var prisonersQuery = _dbContext.Prisoners.Where(x => !x.JailId.HasValue)
            .OrderByDescending(x => x.PrisonerFunction.LastReleasedOn.Value)
            .Select(x => new ReleasedPrisonerListDto
            {
                FullName = x.FullName,
                LastReleasedOn = x.PrisonerFunction.LastReleasedOn.Value.ToLocalDateTimeString(),
                Pid = x.Pid,
                PrisonerId = x.Id
            });

        if (!string.IsNullOrEmpty(request.Pid))
        {
            prisonersQuery = prisonersQuery.Where(x => x.Pid.Contains(request.Pid));
        }

        var countQuery = prisonersQuery.AsQueryable();

        if (request.Skip.HasValue)
        {
            prisonersQuery = prisonersQuery.Skip(request.Skip.Value);
        }

        if (request.Take.HasValue)
        {
            prisonersQuery = prisonersQuery.Take(request.Take.Value);
        }

        var prisoners = await prisonersQuery.ToListAsync(cancellationToken);
        var count = await countQuery.CountAsync(cancellationToken);
        return new(prisoners, count);
    }
}
