using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Dto.Prison;
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

    public PrisonerDetailsQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResponseDto<PrisonerDetailDto>> Handle(PrisonerDetailsQuery request, CancellationToken cancellationToken)
    {
        var prisoner = await _dbContext.Prisoners
            .Select(x => new PrisonerDetailDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                FullName = x.FullName,
                JailName = x.Jail.Name,
                JailCode = x.Jail.Code,
                LastName = x.LastName,
                MiddleName = x.MiddleName,
                Pid = x.Pid,
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
            .SingleOrDefaultAsync(x => x.Id == request.Id);

        if (prisoner is null)
        {
            throw new AppException("Unknown prisoner");
        }

        prisoner.AddressAsText = string.Join(", ",
            new string[] {
                prisoner.Address.HouseName ?? string.Empty,
                prisoner.Address.Street ?? string.Empty,
                prisoner.Address.City ?? string.Empty,
                prisoner.Address.State ?? string.Empty,
                prisoner.Address.Country ?? string.Empty,
                prisoner.Address.PinCode ?? string.Empty,
            }
            .Where(x=> !string.IsNullOrEmpty(x)));
        return new ResponseDto<PrisonerDetailDto>(prisoner);
    }
}

