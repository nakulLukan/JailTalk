using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Storage;
using JailTalk.Application.Dto.System;
using JailTalk.Application.Services;
using JailTalk.Shared.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prisoner.Attachments;

public class GetPrisonerImagesQuery : IRequest<List<ImageViewDto>>
{
    public Guid PrisonerId { get; set; }
}

public class GetPrisonerImagesQueryHandler : IRequestHandler<GetPrisonerImagesQuery, List<ImageViewDto>>
{
    readonly IAppDbContext _dbContext;
    readonly IFileStorage _fileStorage;

    public GetPrisonerImagesQueryHandler(IAppDbContext dbContext, IFileStorage fileStorage)
    {
        _dbContext = dbContext;
        _fileStorage = fileStorage;
    }

    public async Task<List<ImageViewDto>> Handle(GetPrisonerImagesQuery request, CancellationToken cancellationToken)
    {
        var images = await _dbContext.PrisonerFaceEncodingMappings.Where(x => x.PrisonerId == request.PrisonerId)
            .Select(x => new
            {
                x.ImageId,
                x.PrisonerId,
                x.Attachment.RelativeFilePath,
                x.Attachment.FileName
            })
            .ToArrayAsync(cancellationToken);
        List<ImageViewDto> result = new List<ImageViewDto>();
        foreach (var image in images)
        {
            result.Add(new ImageViewDto
            {
                ImageSrc = _fileStorage.GetPresignedUrl(AttachmentHelper.GenerateFullPath(image.RelativeFilePath, image.FileName)),
                FileName = image.FileName,
                Id = image.ImageId
            });
        }

        return result;
    }
}

