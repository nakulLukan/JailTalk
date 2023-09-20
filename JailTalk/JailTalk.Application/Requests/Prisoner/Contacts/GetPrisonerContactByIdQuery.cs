using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Dto.Prison;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prisoner.Contacts;

public class GetPrisonerContactByIdQuery : IRequest<PrisonerContactDetailDto>
{
    public long ContactId { get; set; }
}

public class GetPrisonerContactByIdQueryHandler : IRequestHandler<GetPrisonerContactByIdQuery, PrisonerContactDetailDto>
{
    readonly IAppDbContext _dbContext;

    public GetPrisonerContactByIdQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PrisonerContactDetailDto> Handle(GetPrisonerContactByIdQuery request, CancellationToken cancellationToken)
    {
        var contact = await _dbContext.PhoneDirectory.Where(x => x.Id == request.ContactId)
            .Select(x => new PrisonerContactDetailDto
            {
                Id = x.Id,
                CountryCode = x.CountryCode.Replace("+", ""),
                Name = x.Name,
                PhoneNumber = x.PhoneNumber,
                RelativeTypeId = x.RelativeTypeId
            })
            .SingleOrDefaultAsync(cancellationToken) ?? throw new AppException("Contact not found");

        return contact;
    }
}
