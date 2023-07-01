using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Prison;
using JailTalk.Domain.Prison;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prison;

public class AddContactDetailsCommand : AddContactDetailsDto, IRequest<ResponseDto<long>>
{
}
public class AddContactDetailsCommandHandler : IRequestHandler<AddContactDetailsCommand, ResponseDto<long>>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;

    public AddContactDetailsCommandHandler(IAppDbContext dbContext, IAppRequestContext requestContext)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
    }

    public async Task<ResponseDto<long>> Handle(AddContactDetailsCommand request, CancellationToken cancellationToken)
    {
        string absolutePhoneNumber = request.CountryCode + request.PhoneNumber;
        if (await _dbContext.PhoneDirectory.AnyAsync(x => x.PrisonerId == request.PrisonerId
            && x.CountryCode == request.CountryCode
            && x.PhoneNumber == request.PhoneNumber))
        {
            throw new AppException("Given contact number already exists");
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
            }
        };

        _dbContext.PhoneDirectory.Add(entry);
        await _dbContext.SaveAsync(cancellationToken);
        return new(entry.Id);
    }
}

