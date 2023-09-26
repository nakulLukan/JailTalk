using JailTalk.Application.Dto.Lookup;
using JailTalk.Domain.Lookup;

namespace JailTalk.Application.Services;

public class AddressHelper
{
    public static AddressBook FromNewAddress(NewAddressDto addressDto)
    {
        if (addressDto is null) return null;
        return new()
        {
            City = addressDto.City,
            CountryId = addressDto.CountryId,
            HouseName = addressDto.HouseName,
            PinCode = addressDto.PinCode,
            StateId = addressDto.StateId,
            Street = addressDto.Street,
        };
    }

    public static NewAddressDto ToNewAddressDto(AddressBook address)
    {
        if (address == null) return new NewAddressDto();
        return new()
        {
            City = address.City,
            CountryId = address.CountryId,
            HouseName = address.HouseName,
            PinCode = address.PinCode,
            StateId = address.StateId,
            Street = address.Street,
        };
    }
}
