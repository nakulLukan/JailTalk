using JailTalk.Application.Contracts.Data;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prison;

public class UnLinkAttachmentFromPrisonerCommand : IRequest<ResponseDto<bool>>
{
    public Guid PrisonerId { get; set; }
    public int ImageId { get; set; }
}

public class UnLinkAttachmentFromPrisonerCommandHandler : IRequestHandler<UnLinkAttachmentFromPrisonerCommand, ResponseDto<bool>>
{
    readonly IAppDbContext _appDbContext;

    public UnLinkAttachmentFromPrisonerCommandHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<ResponseDto<bool>> Handle(UnLinkAttachmentFromPrisonerCommand request, CancellationToken cancellationToken)
    {
        var prisonerFaceEncoding = await _appDbContext.PrisonerFaceEncodingMappings.AsTracking()
            .Include(x => x.Attachment)
            .FirstOrDefaultAsync(x => x.PrisonerId == request.PrisonerId
                && x.ImageId == request.ImageId);
        if (prisonerFaceEncoding is null)
        {
            throw new AppException("Image not found.");
        }

        _appDbContext.PrisonerFaceEncodingMappings.Remove(prisonerFaceEncoding);
        _appDbContext.Attachments.Remove(prisonerFaceEncoding.Attachment);
        await _appDbContext.SaveAsync(cancellationToken);
        return new(true);
    }
}

