using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Dto.System;
using JailTalk.Shared.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prison;

public class GetPrisonerImagesQuery : IRequest<List<ImageViewDto>>
{
    public Guid PrisonerId { get; set; }
}

public class GetPrisonerImagesQueryHandler : IRequestHandler<GetPrisonerImagesQuery, List<ImageViewDto>>
{
    readonly IAppDbContext _dbContext;

    public GetPrisonerImagesQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ImageViewDto>> Handle(GetPrisonerImagesQuery request, CancellationToken cancellationToken)
    {
        var images = await _dbContext.PrisonerFaceEncodingMappings.Where(x => x.PrisonerId == request.PrisonerId)
            .Select(x => new
            {
                x.ImageId,
                x.PrisonerId,
                x.Attachment.Data,
                x.Attachment.FileName
            })
            .ToArrayAsync(cancellationToken);
        List<ImageViewDto> result = new List<ImageViewDto>();
        foreach (var image in images)
        {
            var base64String = image.Data.ConvertByteArrayToImgSrc();
            result.Add(new ImageViewDto
            {
                Base64ImageSrc = base64String,
                FileName = image.FileName,
                Id = image.ImageId
            });
        }

        return result;
    }
}

