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

public class DownloadContactProofQuery : IRequest<List<string>>
{
    public long ContactId { get; set; }
}

public class DownloadContactProofQueryHandler : IRequestHandler<DownloadContactProofQuery, List<string>>
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

    public async Task<List<string>> Handle(DownloadContactProofQuery request, CancellationToken cancellationToken)
    {
        var jailId = _requestContext.GetAssociatedPrisonId();
        var attachments = await _dbContext.PhoneDirectory
            .WhereInPrison(x => x.Prisoner.JailId, jailId)
            .Where(x => x.Id == request.ContactId)
            .SelectMany(x => x.IdProofAttachments)
            .Select(x=> new
            {
                x.Id,
                x.FileName,
                x.RelativeFilePath
            })
            .ToListAsync(cancellationToken) ?? throw new AppException(CommonExceptionMessages.PermissionDenied);

        return attachments.Select(attachment => _fileStorage.GetPresignedUrl(AttachmentHelper.GenerateFullPath(attachment.RelativeFilePath, attachment.FileName)))
            .ToList();
    }
}

