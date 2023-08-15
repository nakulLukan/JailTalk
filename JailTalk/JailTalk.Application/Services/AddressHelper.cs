using JailTalk.Application.Dto.Lookup;
using JailTalk.Domain.Lookup;

namespace JailTalk.Application.Services;

public class AddressHelper
{
    public static AddressBook ToNewAddress(NewAddressDto addressDto)
    {
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
}
