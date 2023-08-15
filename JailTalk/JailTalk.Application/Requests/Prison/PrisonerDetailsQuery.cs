using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Prison;
using JailTalk.Application.Extensions;
using JailTalk.Application.Services;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prison;

public class PrisonerDetailsQuery : IRequest<ResponseDto<PrisonerDetailDto>>
{
    public Guid Id { get; set; }
}

public class PrisonerDetailsQueryHandler : IRequestHandler<PrisonerDetailsQuery, ResponseDto<PrisonerDetailDto>>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;

    public PrisonerDetailsQueryHandler(IAppDbContext dbContext, IAppRequestContext requestContext)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
    }

    public async Task<ResponseDto<PrisonerDetailDto>> Handle(PrisonerDetailsQuery request, CancellationToken cancellationToken)
    {
        var prisonId = _requestContext.GetAssociatedPrisonId();
        var prisoner = await _dbContext.Prisoners
            .WhereInPrison(x => x.JailId, prisonId)
            .Select(x => new PrisonerDetailDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                FullName = x.FullName,
                JailName = x.Jail.Name,
                JailCode = x.Jail.Code,
                LastName = x.LastName,
                MiddleName = x.MiddleName,
                Gender = x.Gender.ToString(),
                Pid = x.Pid,
                HasUnlimitedCallPriviledgeEnabled = PrisonerHelper.IsUnlimitedCallPriviledgeEnabled(x.PrisonerFunction.UnlimitedCallsEndsOn),
                Address = new Dto.Lookup.AddressDetailDto
                {
                    City = x.Address.City,
                    Country = x.Address.Country.Value,
                    State = x.Address.State.Value,
                    HouseName = x.Address.HouseName,
                    PinCode = x.Address.PinCode,
                    Street = x.Address.Street
                },
            })
            .SingleOrDefaultAsync(x => x.Id == request.Id) ?? throw new AppException("Unknown prisoner");
        prisoner.AddressAsText = prisoner.Address.AddressAsText();
        return new ResponseDto<PrisonerDetailDto>(prisoner);
    }
}

