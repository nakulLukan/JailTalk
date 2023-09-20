using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Contracts.Storage;
using JailTalk.Application.Dto.Prison;
using JailTalk.Application.Services;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prisoner;

public class PrisonerDetailsQuery : IRequest<ResponseDto<PrisonerDetailDto>>
{
    public Guid Id { get; set; }
}

public class PrisonerDetailsQueryHandler : IRequestHandler<PrisonerDetailsQuery, ResponseDto<PrisonerDetailDto>>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;
    readonly IFileStorage _fileStorage;

    public PrisonerDetailsQueryHandler(IAppDbContext dbContext, IAppRequestContext requestContext, IFileStorage fileStorage)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
        _fileStorage = fileStorage;
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
                DpSrc = AttachmentHelper.GenerateFullPath(x.PrisonerFunction.DpAttachment.RelativeFilePath, x.PrisonerFunction.DpAttachment.FileName),
                HasUnlimitedCallPriviledgeEnabled = PrisonerHelper.IsUnlimitedCallPriviledgeEnabled(x.PrisonerFunction.UnlimitedCallsEndsOn),
            })
            .SingleOrDefaultAsync(x => x.Id == request.Id) ?? throw new AppException("Unknown prisoner");
        prisoner.DpSrc = _fileStorage.GetPresignedUrl(prisoner.DpSrc);
        return new ResponseDto<PrisonerDetailDto>(prisoner);
    }
}

