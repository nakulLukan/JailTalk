using FluentValidation;
using JailTalk.Application.Contracts.Audio;
using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Contracts.Storage;
using JailTalk.Domain.Prison;
using JailTalk.Domain.System;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace JailTalk.Application.Requests.Prisoner.CallManagement;

public class SaveCallRecordingCommand : IRequest<ResponseDto<long>>
{
    public byte[] AudioClipData { get; set; }
    public long CallHistoryId { get; set; }
}

public class SaveCallRecordingCommandHandler : IRequestHandler<SaveCallRecordingCommand, ResponseDto<long>>
{
    readonly IAppDbContext _dbContext;
    readonly IDeviceRequestContext _requestContext;
    readonly ILogger<SaveCallRecordingCommandHandler> _logger;
    readonly IFileStorage _fileStorage;
    readonly IAudioService _audioService;

    public SaveCallRecordingCommandHandler(
        IDeviceRequestContext requestContext,
        IAppDbContext dbContext,
        IFileStorage fileStorage,
        ILogger<SaveCallRecordingCommandHandler> logger,
        IAudioService audioService)
    {
        _requestContext = requestContext;
        _dbContext = dbContext;
        _fileStorage = fileStorage;
        _logger = logger;
        _audioService = audioService;
    }

    public async Task<ResponseDto<long>> Handle(SaveCallRecordingCommand request, CancellationToken cancellationToken)
    {
        var prisonerId = _requestContext.GetPrisonerId();
        var callHistory = await _dbContext.CallHistory
            .Where(x => x.Id == request.CallHistoryId)
            .Select(x => new
            {
                x.Id,
                x.CallRecordingAttachmentId,
                x.PhoneDirectory.Prisoner.Pid,
                x.PhoneDirectory.IsCallRecordingAllowed
            })
            .FirstOrDefaultAsync(cancellationToken) ?? throw new AppApiException(HttpStatusCode.NotFound, "SCR-E-003", "Invalid call history ID.");

        if (callHistory.CallRecordingAttachmentId.HasValue)
        {
            throw new AppApiException(HttpStatusCode.BadRequest, "SCR-E-001", "A recording already exists");
        }

        if (!_audioService.IsValidAudioFile(request.AudioClipData))
        {
            throw new AppApiException(HttpStatusCode.BadRequest, "SCR-E-002", "The audio clip is not in a valid audio format.");
        }
        // If call recording is disabled then do not save the recording.
        if (!callHistory.IsCallRecordingAllowed)
        {
            throw new AppApiException(HttpStatusCode.BadRequest, "SCR-E-003", "Call recording is not enabled for this contact.");
        }

        string fileName = $"recording_{callHistory.Pid}_{AppDateTime.UtcNow.ToFileTimeString()}.mp3";
        string filePath = $"prisoners/{callHistory.Pid}";
        var response = await _fileStorage.UploadFile(request.AudioClipData, fileName, filePath, cancellationToken);
        _logger.LogInformation("Call recording saved successfully. Uploaded to path {path}", response.RelativePath);

        Attachment attachment = await SaveAudioAsAttachment(request, fileName, filePath, cancellationToken);
        return new(attachment.Id);
    }

    private async Task<Attachment> SaveAudioAsAttachment(SaveCallRecordingCommand request, string fileName, string filePath, CancellationToken cancellationToken)
    {
        CallHistory callHistoryEntity = new CallHistory()
        {
            Id = request.CallHistoryId,
        };

        var attachment = new Attachment
        {
            FileName = fileName,
            RelativeFilePath = filePath + "/" + fileName,
            IsBlob = false,
            FileSizeInBytes = request.AudioClipData.Length,
        };

        _dbContext.Attachments.Add(attachment);
        await _dbContext.SaveAsync(cancellationToken);

        var updateCount = await _dbContext.CallHistory
            .Where(x => x.Id == request.CallHistoryId)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.CallRecordingAttachmentId, attachment.Id));
        _logger.LogInformation("Audio added as attachment with Id: {id}", attachment.Id);
        await _dbContext.SaveAsync(cancellationToken);
        return attachment;
    }
}

public class SaveCallRecordingCommandValidator : AbstractValidator<SaveCallRecordingCommand>
{
    public SaveCallRecordingCommandValidator()
    {
        RuleFor(x => x.AudioClipData)
            .NotNull()
            .Must(x => x.Length <= 15_000_000)
                .WithMessage("File size too long. Try to upload file less than 15 MB.");
    }
}
