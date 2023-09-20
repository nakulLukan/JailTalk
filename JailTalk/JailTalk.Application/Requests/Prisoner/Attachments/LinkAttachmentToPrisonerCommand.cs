using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Domain.Prison;
using JailTalk.Shared;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prisoner.Attachments;

public class LinkAttachmentToPrisonerCommand : IRequest<ResponseDto<bool>>
{
    public Guid PrisonerId { get; set; }
    public List<int> AttachmentIds { get; set; }
}

public class LinkAttachmentToPrisonerCommandHandler : IRequestHandler<LinkAttachmentToPrisonerCommand, ResponseDto<bool>>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;

    public LinkAttachmentToPrisonerCommandHandler(IAppDbContext dbContext, IAppRequestContext requestContext)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
    }

    public async Task<ResponseDto<bool>> Handle(LinkAttachmentToPrisonerCommand request, CancellationToken cancellationToken)
    {
        var userId = await _requestContext.GetUserId();
        var prisoner = await _dbContext.Prisoners
            .Where(x => x.Id == request.PrisonerId)
            .Select(x => new
            {
                x.FullName,
                x.PrisonerFunction.DpAttachmentId
            })
            .FirstOrDefaultAsync(cancellationToken) ?? throw new AppException(CommonExceptionMessages.UserNotFound);

        // Link each attachment to prisoner
        foreach (var imageId in request.AttachmentIds)
        {
            var imageByteArray = await _dbContext.Attachments
                .Where(x => !x.IsBlob && x.Id == imageId)
                .Select(x => x.Data)
                .FirstAsync(cancellationToken);

            PrisonerFaceEncodingMapping entity = new()
            {
                ImageId = imageId,
                PrisonerId = request.PrisonerId,
            };
            await _dbContext.PrisonerFaceEncodingMappings.AddAsync(entity);
        }
        await _dbContext.SaveAsync(cancellationToken);

        // If the prisoner does not have a dp then use the first image
        if (!prisoner.DpAttachmentId.HasValue)
        {
            await SetPrisonerDp(request);
        }

        return new(true);
    }

    private async Task SetPrisonerDp(LinkAttachmentToPrisonerCommand request)
    {
        var firstAttachmentId = request.AttachmentIds.OrderByDescending(x => x).First();
        await _dbContext.PrisonerFunctions.Where(x => x.PrisonerId == request.PrisonerId)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.DpAttachmentId, firstAttachmentId));
    }
}
