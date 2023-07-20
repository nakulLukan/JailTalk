using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Prison;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prison;

public class PrisonerAllActiveContactsQuery : IRequest<List<ShowContactsDto>>
{
}

public class PrisonerAllActiveContactsQueryHandler : IRequestHandler<PrisonerAllActiveContactsQuery, List<ShowContactsDto>>
{
    readonly IAppDbContext _dbContext;
    readonly IDeviceRequestContext _requestContext;

    public PrisonerAllActiveContactsQueryHandler(IAppDbContext dbContext, IDeviceRequestContext requestContext)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
    }

    public async Task<List<ShowContactsDto>> Handle(PrisonerAllActiveContactsQuery request, CancellationToken cancellationToken)
    {
        var prisonerId = _requestContext.GetPrisonerId();
        var contacts = await _dbContext.PhoneDirectory
            .Where(x => x.PrisonerId == prisonerId
                && x.IsActive
                && !x.IsBlocked)
            .OrderBy(x => x.IsActive)
                .ThenBy(x => x.RelativeType.Value)
            .Select(x => new ShowContactsDto
            {
                Id = x.Id,
                ContactRelationName = x.RelativeType.Value,
                CountryCode = x.CountryCode,
                PhoneNumber = x.PhoneNumber,
            })
            .ToListAsync(cancellationToken);
        return contacts;
    }
}

