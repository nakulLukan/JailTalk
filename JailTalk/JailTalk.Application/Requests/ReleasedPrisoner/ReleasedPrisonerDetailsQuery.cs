using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Dto.ReleasedPrisoner;
using JailTalk.Application.Extensions;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.ReleasedPrisoner;

public class ReleasedPrisonerDetailsQuery : IRequest<ResponseDto<ReleasedPrisonerDetailDto>>
{
    public Guid Id { get; set; }
}

public class ReleasedPrisonerDetailsQueryHandler : IRequestHandler<ReleasedPrisonerDetailsQuery, ResponseDto<ReleasedPrisonerDetailDto>>
{
    readonly IAppDbContext _dbContext;

    public ReleasedPrisonerDetailsQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResponseDto<ReleasedPrisonerDetailDto>> Handle(ReleasedPrisonerDetailsQuery request, CancellationToken cancellationToken)
    {
        var prisoner = await _dbContext.Prisoners
            .Select(x => new
            {
                Id = x.Id,
                FirstName = x.FirstName,
                FullName = x.FullName,
                LastJailName = x.PrisonerFunction.LastAssociatedJail.Name,
                LastJailCode = x.PrisonerFunction.LastAssociatedJail.Code,
                LastName = x.LastName,
                MiddleName = x.MiddleName,
                Gender = x.Gender.ToString(),
                Pid = x.Pid,
                LastReleasedOn = x.PrisonerFunction.LastReleasedOn,
            })
            .SingleOrDefaultAsync(x => x.Id == request.Id) ?? throw new AppException("Unknown prisoner");
        return new ResponseDto<ReleasedPrisonerDetailDto>(new ReleasedPrisonerDetailDto()
        {
            Id = prisoner.Id,
            FirstName = prisoner.FirstName,
            LastJailCode = prisoner.LastJailCode,
            FullName = prisoner.FullName,
            Gender = prisoner.Gender.ToString(),
            LastJailName = prisoner.LastName,
            LastName = prisoner.LastName,
            LastReleasedOnAsText = prisoner.LastReleasedOn.Value.ToLocalDateTimeString(),
            MiddleName = prisoner.MiddleName,
            Pid = prisoner.Pid
        });
    }
}

