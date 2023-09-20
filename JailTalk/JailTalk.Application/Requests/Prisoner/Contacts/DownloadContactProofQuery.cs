using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Contracts.Storage;
using JailTalk.Application.Services;
using JailTalk.Shared;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prisoner.Contacts;

public class DownloadContactProofQuery : IRequest<string>
{
    public long ContactId { get; set; }
}

public class DownloadContactProofQueryHandler : IRequestHandler<DownloadContactProofQuery, string>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;
    readonly IFileStorage _fileStorage;

    public DownloadContactProofQueryHandler(IAppDbContext dbContext, IAppRequestContext requestContext, IFileStorage fileStorage)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
        _fileStorage = fileStorage;
    }

    public async Task<string> Handle(DownloadContactProofQuery request, CancellationToken cancellationToken)
    {
        var jailId = _requestContext.GetAssociatedPrisonId();
        var attachment = await _dbContext.PhoneDirectory
            .WhereInPrison(x => x.Prisoner.JailId, jailId)
            .Select(x => new
            {
                x.Id,
                x.IdProofAttachment.FileName,
                x.IdProofAttachment.RelativeFilePath
            })
            .FirstOrDefaultAsync(x => x.Id == request.ContactId, cancellationToken) ?? throw new AppException(CommonExceptionMessages.PermissionDenied);

        return _fileStorage.GetPresignedUrl(AttachmentHelper.GenerateFullPath(attachment.RelativeFilePath, attachment.FileName));
    }
}

