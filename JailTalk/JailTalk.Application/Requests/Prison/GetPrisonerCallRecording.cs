using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Contracts.Storage;
using JailTalk.Application.Dto.Prison;
using JailTalk.Domain.Prison;
using JailTalk.Shared;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prison;

public class GetPrisonerCallRecording : IRequest<PrisonerCallRecordingResponseDto>
{
    public long CallHistoryId { get; set; }
}

public class GetPrisonerCallRecordingHandler : IRequestHandler<GetPrisonerCallRecording, PrisonerCallRecordingResponseDto>
{
    readonly IAppRequestContext _requestContext;
    readonly IAppDbContext _dbContext;
    readonly IFileStorage _fileStorage;

    public GetPrisonerCallRecordingHandler(
        IAppDbContext dbContext,
        IAppRequestContext requestContext,
        IFileStorage fileStorage)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
        _fileStorage = fileStorage;
    }

    public async Task<PrisonerCallRecordingResponseDto> Handle(GetPrisonerCallRecording request, CancellationToken cancellationToken)
    {
        var prisonId = _requestContext.GetAssociatedPrisonId();
        var callRecording = await _dbContext.CallHistory
            .Include(x => x.CallRecordingAttachment)
            .WhereInPrison(x => x.PhoneDirectory.Prisoner.JailId, prisonId)
            .FirstOrDefaultAsync(x => x.Id == request.CallHistoryId, cancellationToken) ?? throw new AppException(CommonExceptionMessages.ItemNotFound);

        var callDuration = (callRecording.EndedOn - callRecording.CallStartedOn).ToHoursMinutesSeconds();
        string signedUrl = GetPresingedUrl(callRecording);
        return new PrisonerCallRecordingResponseDto()
        {
            FileName = callRecording.CallRecordingAttachment.FileName,
            FileSizeAsText = callRecording.CallRecordingAttachment.FileSizeInBytes.FileSizeAsString(),
            SignedUrl = signedUrl,
            CallDurationAsText = callDuration,
        };
    }

    private string GetPresingedUrl(CallHistory callRecording)
    {
        if (callRecording.CallRecordingAttachment is null)
        {
            throw new AppException("Could not find the call recording.");
        }
        if (!callRecording.CallRecordingAttachment.IsBlob && string.IsNullOrEmpty(callRecording.CallRecordingAttachment.RelativeFilePath))
        {
            throw new AppException("File url not found for this call record.");
        }
        var signedUrl = _fileStorage.GetPresignedUrl(callRecording.CallRecordingAttachment.RelativeFilePath);
        return signedUrl;
    }
}
