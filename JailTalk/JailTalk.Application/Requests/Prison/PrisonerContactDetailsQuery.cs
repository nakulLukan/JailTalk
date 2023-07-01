using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Dto.Prison;
using JailTalk.Application.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prison;

public class PrisonerContactDetailsQuery : IRequest<List<PrisonerContactDetailListDto>>
{
    public Guid PrisonerId { get; set; }
}

public class PrisonerContactDetailsQueryHandler : IRequestHandler<PrisonerContactDetailsQuery, List<PrisonerContactDetailListDto>>
{
    readonly IAppDbContext _dbContext;

    public PrisonerContactDetailsQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<PrisonerContactDetailListDto>> Handle(PrisonerContactDetailsQuery request, CancellationToken cancellationToken)
    {
        var contactDetails = await _dbContext.PhoneDirectory
            .Include(x=>x.RelativeAddress)
            .Include(x=>x.RelativeType)
            .Where(x => x.PrisonerId == request.PrisonerId)
            .ToListAsync(cancellationToken);
        int index = 1;
        return contactDetails
            .Select(x => new PrisonerContactDetailListDto
            {
                Serial = index++,
                Id = x.Id,
                ContactNumber = string.Join(" ", new string[]
                {
                    x.CountryCode,
                    x.PhoneNumber
                }),
                Status = ConvertToStatus(x.IsActive, x.IsBlocked),
                Relationship = x.RelativeType.Value,
                RelativeAddress = x.RelativeAddress.AddressAsText()
            }).ToList();
    }

    private string ConvertToStatus(bool isActive, bool isBlocked)
    {
        return isActive switch
        {
            true when !isBlocked => "Enabled",
            false when !isBlocked => "Disabled",
            _ => "Blocked"
        };
    }
}

