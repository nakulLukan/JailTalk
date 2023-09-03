using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Prison;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prison;

public class EditContactDetailsCommand : EditContactDetailsDto, IRequest<ResponseDto<long>>
{
}
public class EditContactDetailsCommandHandler : IRequestHandler<EditContactDetailsCommand, ResponseDto<long>>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;
    readonly IApplicationSettingsProvider _settingsProvider;

    public EditContactDetailsCommandHandler(IAppDbContext dbContext, IAppRequestContext requestContext, IApplicationSettingsProvider settingsProvider)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
        _settingsProvider = settingsProvider;
    }

    public async Task<ResponseDto<long>> Handle(EditContactDetailsCommand request, CancellationToken cancellationToken)
    {
        string absolutePhoneNumber = request.CountryCode + request.PhoneNumber;
        if (await _dbContext.PhoneDirectory.AnyAsync(x => x.Id != request.ContactId
            && x.PrisonerId == request.PrisonerId
            && x.CountryCode == request.CountryCode
            && x.PhoneNumber == request.PhoneNumber))
        {
            throw new AppException("Given contact number already exists");
        }

        var userId = await _requestContext.GetUserId();
        var contact = await _dbContext.PhoneDirectory.AsTracking()
            .SingleOrDefaultAsync(x => x.Id == request.ContactId, cancellationToken) ?? throw new AppException("Contact record could not be found");
        contact.CountryCode = request.CountryCode;
        contact.PhoneNumber = request.PhoneNumber;
        contact.Name = request.Name;
        contact.RelativeTypeId = request.RelationshipId.Value;
        contact.UpdatedBy = userId;
        contact.UpdatedOn = AppDateTime.UtcNow;

        await _dbContext.SaveAsync(cancellationToken);
        return new(contact.Id);
    }
}

