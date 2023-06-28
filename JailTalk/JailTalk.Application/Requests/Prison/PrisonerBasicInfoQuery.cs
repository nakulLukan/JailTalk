using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Dto.Prison;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prison;

public class PrisonerBasicInfoQuery : IRequest<PrisonerBasicInfoDto>
{
    public Guid PrisonerId { get; set; }
}

public class PrisonerBasicInfoQueryHandler : IRequestHandler<PrisonerBasicInfoQuery, PrisonerBasicInfoDto>
{
    readonly IAppDbContext _dbContext;

    public PrisonerBasicInfoQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PrisonerBasicInfoDto> Handle(PrisonerBasicInfoQuery request, CancellationToken cancellationToken)
    {
        var basicInfo = await _dbContext.Prisoners
            .Where(x => x.Id == request.PrisonerId)
            .Select(x => new PrisonerBasicInfoDto
            {
                Id = x.Id,
                FullName = x.FullName
            })
            .SingleOrDefaultAsync(cancellationToken);
        if (basicInfo is null)
        {
            throw new AppException("Unknown user");
        }

        return basicInfo;
    }
}

