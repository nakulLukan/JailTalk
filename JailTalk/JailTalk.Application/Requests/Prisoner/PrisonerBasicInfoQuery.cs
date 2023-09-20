using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Prison;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prisoner;

public class PrisonerBasicInfoQuery : IRequest<PrisonerBasicInfoDto>
{
    public Guid PrisonerId { get; set; }
}

public class PrisonerBasicInfoQueryHandler : IRequestHandler<PrisonerBasicInfoQuery, PrisonerBasicInfoDto>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;

    public PrisonerBasicInfoQueryHandler(IAppDbContext dbContext, IAppRequestContext requestContext)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
    }

    public async Task<PrisonerBasicInfoDto> Handle(PrisonerBasicInfoQuery request, CancellationToken cancellationToken)
    {
        var basicInfo = await _dbContext.Prisoners
            .WhereInPrison(x => x.JailId, _requestContext.GetAssociatedPrisonId())
            .Where(x => x.Id == request.PrisonerId)
            .Select(x => new PrisonerBasicInfoDto
            {
                Id = x.Id,
                FullName = x.FullName
            })
            .SingleOrDefaultAsync(cancellationToken);
        if (basicInfo is null)
        {
            throw new AppException("Unknown user", true);
        }

        return basicInfo;
    }
}

