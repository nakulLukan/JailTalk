using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Jail;
using JailTalk.Application.Services;
using JailTalk.Shared;
using JailTalk.Shared.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Jail;

public class GetPrisonByIdQuery : IRequest<ViewPrisonDto>
{
    public int JailId { get; set; }
}
public class GetPrisonByIdQueryHandler : IRequestHandler<GetPrisonByIdQuery, ViewPrisonDto>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;

    public GetPrisonByIdQueryHandler(IAppDbContext dbContext, IAppRequestContext requestContext)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
    }

    public async Task<ViewPrisonDto> Handle(GetPrisonByIdQuery request, CancellationToken cancellationToken)
    {
        var associatedJailid = _requestContext.GetAssociatedPrisonId();
        var jail = await _dbContext.Jails
            .Include(x => x.Address)
            .WhereInPrison(x => x.Id, associatedJailid)
            .Select(x => new ViewPrisonDto
            {
                Address = AddressHelper.ToNewAddressDto(x.Address),
                ContactEmailAddress = x.ContactEmailAddress,
                JailId = x.Id,
                PrisonCode = x.Code,
                PrisonName = x.Name
            })
            .FirstOrDefaultAsync(x => x.JailId == request.JailId, cancellationToken) ?? throw new ApplicationException(CommonExceptionMessages.PermissionDenied);
        return jail;
    }
}

