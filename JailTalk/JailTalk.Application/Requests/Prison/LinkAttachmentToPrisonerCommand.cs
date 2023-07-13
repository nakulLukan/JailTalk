using JailTalk.Application.Contracts.AI;
using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Domain.Prison;
using JailTalk.Shared;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prison;

public class LinkAttachmentToPrisonerCommand : IRequest<ResponseDto<bool>>
{
    public Guid PrisonerId { get; set; }
    public List<int> AttachmentIds { get; set; }
}

public class LinkAttachmentToPrisonerCommandHandler : IRequestHandler<LinkAttachmentToPrisonerCommand, ResponseDto<bool>>
{
    readonly IAppDbContext _dbContext;
    readonly IAppFaceRecognition _faceRecognition;
    readonly IAppRequestContext _requestContext;

    public LinkAttachmentToPrisonerCommandHandler(IAppDbContext dbContext, IAppFaceRecognition faceRecognition, IAppRequestContext requestContext)
    {
        _dbContext = dbContext;
        _faceRecognition = faceRecognition;
        _requestContext = requestContext;
    }

    public async Task<ResponseDto<bool>> Handle(LinkAttachmentToPrisonerCommand request, CancellationToken cancellationToken)
    {
        var userId = await _requestContext.GetUserId();
        var prisonerFullName = await _dbContext.Prisoners
            .Where(x => x.Id == request.PrisonerId)
            .Select(x => x.FullName)
            .FirstOrDefaultAsync(cancellationToken);
        if (prisonerFullName == null)
        {
            throw new AppException(CommonExceptionMessages.UserNotFound);
        }
        foreach (var imageId in request.AttachmentIds)
        {
            var imageByteArray = await _dbContext.Attachments
                .Where(x => x.IsBlob && x.Id == imageId)
                .Select(x => x.Data)
                .FirstAsync(cancellationToken);

            var faceEncoding = _faceRecognition.GetFaceEncodings(imageByteArray);
            PrisonerFaceEncodingMapping entity = new()
            {
                FaceEncoding = new Domain.Identity.AppFaceEncoding
                {
                    Encoding = faceEncoding,
                    IsActive = true,
                    LastModifiedBy = userId,
                    LastModifiedOn = AppDateTime.UtcNow,
                    EncodingName = $"{prisonerFullName} - {imageId}"
                },
                ImageId = imageId,
                PrisonerId = request.PrisonerId,
            };
            await _dbContext.PrisonerFaceEncodingMappings.AddAsync(entity);
        }

        await _dbContext.SaveAsync(cancellationToken);
        return new(true);
    }
}
