using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Storage;
using JailTalk.Application.Services;
using JailTalk.Domain.Prison;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JailTalk.Application.Requests.Prisoner.Attachments;

public class UnLinkAttachmentFromPrisonerCommand : IRequest<ResponseDto<bool>>
{
    public Guid PrisonerId { get; set; }
    public int ImageId { get; set; }
}

public class UnLinkAttachmentFromPrisonerCommandHandler : IRequestHandler<UnLinkAttachmentFromPrisonerCommand, ResponseDto<bool>>
{
    readonly IAppDbContext _appDbContext;
    readonly IFileStorage _fileStorage;
    readonly ILogger<UnLinkAttachmentFromPrisonerCommandHandler> _logger;

    public UnLinkAttachmentFromPrisonerCommandHandler(IAppDbContext appDbContext, IFileStorage fileStorage, ILogger<UnLinkAttachmentFromPrisonerCommandHandler> logger)
    {
        _appDbContext = appDbContext;
        _fileStorage = fileStorage;
        _logger = logger;
    }

    public async Task<ResponseDto<bool>> Handle(UnLinkAttachmentFromPrisonerCommand request, CancellationToken cancellationToken)
    {
        PrisonerFaceEncodingMapping prisonerFaceEncoding = await GetPrisonerFaceEncoding(request);
        var prisonerDp = prisonerFaceEncoding.Prisoner.PrisonerFunction.DpAttachmentId;
        if (prisonerDp.HasValue && prisonerDp.Value == request.ImageId)
        {
            await ChangeDp(request, cancellationToken);
        }
        await DeleteRecords(prisonerFaceEncoding, cancellationToken);

        return new(true);
    }

    private async Task<PrisonerFaceEncodingMapping> GetPrisonerFaceEncoding(UnLinkAttachmentFromPrisonerCommand request)
    {
        return await _appDbContext.PrisonerFaceEncodingMappings.AsTracking()
                    .Include(x => x.Attachment)
                    .Include(x => x.Prisoner.PrisonerFunction)
                    .FirstOrDefaultAsync(x => x.PrisonerId == request.PrisonerId
                        && x.ImageId == request.ImageId) ?? throw new AppException("Image not found.");
    }

    private async Task ChangeDp(UnLinkAttachmentFromPrisonerCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Removing prisoner current dp #{id}", request.ImageId);
        // Find the next image linked to the prisoner
        var newDpImageId = await _appDbContext.PrisonerFaceEncodingMappings
            .Where(x => x.PrisonerId == request.PrisonerId && x.ImageId != request.ImageId)
            .OrderBy(x => x.Id)
            .Select(x => x.ImageId)
            .FirstOrDefaultAsync(cancellationToken);

        // Update it with the new attachment.
        if (newDpImageId > 0)
        {
            var updateCount = await _appDbContext.PrisonerFunctions.Where(x => x.PrisonerId == request.PrisonerId)
                .ExecuteUpdateAsync(x => x.SetProperty(y => y.DpAttachmentId, newDpImageId));
            _logger.LogInformation("Updating dp with new image #{id}; Records Updated: {count}", newDpImageId, updateCount);
        }
    }

    private async Task DeleteRecords(PrisonerFaceEncodingMapping prisonerFaceEncoding, CancellationToken cancellationToken)
    {
        // Delete the records from db and storage
        await DeleteInStorage(prisonerFaceEncoding);
        _appDbContext.PrisonerFaceEncodingMappings.Remove(prisonerFaceEncoding);
        _appDbContext.Attachments.Remove(prisonerFaceEncoding.Attachment);
        await _appDbContext.SaveAsync(cancellationToken);
    }

    private async Task DeleteInStorage(PrisonerFaceEncodingMapping prisonerFaceEncoding)
    {
        var attachment = prisonerFaceEncoding.Attachment;
        string objectKey = AttachmentHelper.GenerateFullPath(attachment.RelativeFilePath, attachment.FileName);
        try
        {
            await _fileStorage.DeleteFile(objectKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete file {filePath} in the storage", objectKey);
        }
    }
}

