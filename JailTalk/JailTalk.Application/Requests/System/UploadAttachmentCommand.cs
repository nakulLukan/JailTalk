using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Graphics;
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
    public UploadAttachmentCommandHandler(IAppImageEditor appImageEditor, IAppDbContext dbContext)
    {
        _appImageEditor = appImageEditor;
        _dbContext = dbContext;
    }

    public async Task<ResponseDto<int>> Handle(UploadAttachmentCommand request, CancellationToken cancellationToken)
    {
        byte[] data = request.Data;
        if (request.SaveAsThumbnail)
        {
            data = _appImageEditor.ConvertImageToThumbnail(data, 64, 64);
        }
        var attachment = new Domain.System.Attachment
        {
            Data = data,
            FileName = request.FileName,
            IsBlob = true,
            RelativeFilePath = null,
            FileSizeInBytes = data.Length,
        };
        _dbContext.Attachments.Add(attachment);

        await _dbContext.SaveAsync(cancellationToken);
        return new(attachment.Id);
    }
}
