using FluentValidation;
using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Prison;
using JailTalk.Application.Requests.System;
using JailTalk.Application.Services;
using JailTalk.Domain.Prison;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace JailTalk.Application.Requests.Prisoner.Contacts;

public class AddContactDetailsCommand : AddContactDetailsDto, IRequest<ResponseDto<long>>
{
}

public class AddContactDetailsCommandHandler : IRequestHandler<AddContactDetailsCommand, ResponseDto<long>>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;
    readonly IApplicationSettingsProvider _settingsProvider;
    readonly IMediator _mediator;

    public AddContactDetailsCommandHandler(IAppDbContext dbContext, IAppRequestContext requestContext, IApplicationSettingsProvider settingsProvider, IMediator mediator)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
        _settingsProvider = settingsProvider;
        _mediator = mediator;
    }

    public async Task<ResponseDto<long>> Handle(AddContactDetailsCommand request, CancellationToken cancellationToken)
    {
        await new AddContactDetailsCommandValidator().ValidateAndThrowAsync(request, cancellationToken);
        string absolutePhoneNumber = request.CountryCode + request.PhoneNumber;
        if (await _dbContext.PhoneDirectory.AnyAsync(x => x.PrisonerId == request.PrisonerId
            && x.CountryCode == request.CountryCode
            && x.PhoneNumber == request.PhoneNumber))
        {
            throw new AppException("Given contact number already exists");
        }

        var maxEnabledContactsPerPrisoner = await _settingsProvider.GetMaxContactNumbersInActiveState();
        var currentNumberOfActiveContacts = await _dbContext.PhoneDirectory
            .Where(x => x.PrisonerId == request.PrisonerId
                && x.IsActive)
            .CountAsync(cancellationToken);

        if (request.IsEnabled && currentNumberOfActiveContacts >= maxEnabledContactsPerPrisoner)
        {
            var numberOfContactsToBeDisabled = currentNumberOfActiveContacts - maxEnabledContactsPerPrisoner + 1;
            throw new AppException($"Only {maxEnabledContactsPerPrisoner} contacts are allowed to be active. Please disable atleast {numberOfContactsToBeDisabled} contacts.");
        }

        var userId = await _requestContext.GetUserId();
        PhoneDirectory entry = new()
        {
            CountryCode = request.CountryCode,
            PrisonerId = request.PrisonerId,
            PhoneNumber = request.PhoneNumber,
            CreatedBy = userId,
            CreatedOn = AppDateTime.UtcNow,
            IsActive = request.IsEnabled,
            IsBlocked = false,
            LastBlockedOn = null,
            RelativeTypeId = request.RelationshipId.Value,
            UpdatedBy = userId,
            UpdatedOn = AppDateTime.UtcNow,
            RelativeAddress = new Domain.Lookup.AddressBook
            {
                City = request.RelativeAddress.City,
                CountryId = request.RelativeAddress.CountryId,
                StateId = request.RelativeAddress.StateId,
                HouseName = request.RelativeAddress.HouseName,
                PinCode = request.RelativeAddress.PinCode,
            },
            Name = request.Name,
            IdProofTypeId = request.ContactProofTypeId,
            IdProofValue = request.ContactProofValue,
            IsCallRecordingAllowed = request.IsCallRecordingAllowed
        };

        _dbContext.PhoneDirectory.Add(entry);
        await _dbContext.SaveAsync(cancellationToken);
        await CheckAndUploadProof(request, entry, cancellationToken);
        return new(entry.Id);
    }

    private async Task CheckAndUploadProof(AddContactDetailsCommand request, PhoneDirectory entry, CancellationToken cancellationToken)
    {
        if (request.ContactProofAttachment != null && request.ContactProofAttachment.Any())
        {
            var attachmentPath = await PrisonerHelper.GetPrisonerAttachmentBasePath(request.PrisonerId, _dbContext) + "/contacts/proof";

            var attachmentIds = new ConcurrentBag<int>();
            await Task.WhenAll(request.ContactProofAttachment.Select(async file =>
            {
                var response = await _mediator.Send(new UploadAttachmentCommand
                {
                    Data = file.DataStream.ToArray(),
                    FileContent = file.ContentType,
                    FileName = file.FileName,
                    FileDestinationBasePath = attachmentPath,
                    SaveAsThumbnail = false
                });
                attachmentIds.Add(response.Data);
            }));

            entry.IdProofAttachments = await _dbContext.Attachments
                .Where(x => attachmentIds.Contains(x.Id))
                .ToListAsync(cancellationToken);
            await _dbContext.SaveAsync(cancellationToken);
        }
    }
}

public class AddContactDetailsCommandValidator : AbstractValidator<AddContactDetailsCommand>
{
    public AddContactDetailsCommandValidator()
    {
        RuleFor(x => x.ContactProofTypeId)
            .NotEmpty()
                .WithMessage("Choose the ID Proof")
            .NotNull()
                .WithMessage("Choose the ID Proof")
            .When(x => !string.IsNullOrEmpty(x.ContactProofValue));

        RuleFor(x => x.ContactProofValue)
            .NotEmpty()
                .WithMessage("Enter the ID proof value")
            .NotNull()
                .WithMessage("Enter the ID proof value")
            .When(x => x.ContactProofTypeId.HasValue);

        RuleFor(x => x.ContactProofAttachment)
            .NotNull()
                .WithMessage("Please upload the proof.")
            .ForEach(y => y.Must(x => x is not null && x.DataStream is not null && x.DataStream.Length > 0))
                .WithMessage("Please upload the proof.")
            .When(x => x.ContactProofTypeId.HasValue);
    }
}