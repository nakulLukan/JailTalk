using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Graphics;
using JailTalk.Application.Contracts.Storage;
using JailTalk.Application.Dto.System;
using JailTalk.Shared.Models;
using MediatR;

namespace JailTalk.Application.Requests.System;

public class UploadAttachmentCommand : AttachmentUploadRequestDto, IRequest<ResponseDto<int>>
{
}

public class UploadAttachmentCommandHandler : IRequestHandler<UploadAttachmentCommand, ResponseDto<int>>
{
    readonly IAppImageEditor _appImageEditor;
    readonly IAppDbContext _dbContext;
    readonly IFileStorage _fileStorage;

    public UploadAttachmentCommandHandler(IAppImageEditor appImageEditor, IAppDbContext dbContext, IFileStorage fileStorage)
    {
        _appImageEditor = appImageEditor;
        _dbContext = dbContext;
        _fileStorage = fileStorage;
    }

    public async Task<ResponseDto<int>> Handle(UploadAttachmentCommand request, CancellationToken cancellationToken)
    {
        byte[] data = request.Data;
        if (request.SaveAsThumbnail)
        {
            data = _appImageEditor.ConvertImageToThumbnail(data, 64, 64);
        }

        await _fileStorage.UploadFile(data, request.FileName, request.FileDestinationBasePath, cancellationToken);
        var attachment = new Domain.System.Attachment
        {
            Data = null,
            FileName = request.FileName,
            IsBlob = false,
            RelativeFilePath = request.FileDestinationBasePath,
            FileSizeInBytes = data.Length,
        };
        _dbContext.Attachments.Add(attachment);

        await _dbContext.SaveAsync(cancellationToken);
        return new(attachment.Id);
    }
}
