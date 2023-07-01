using JailTalk.Application.Dto.Lookup;

namespace JailTalk.Application.Extensions;

public static class DomainExtension
{
    public static string AddressAsText(this Domain.Lookup.AddressBook address)
    {
        return string.Join(", ",
        new string[] {
                address.HouseName ?? string.Empty,
                address.Street ?? string.Empty,
                address.City ?? string.Empty,
                address.State?.Value ?? string.Empty,
                address.Country?.Value ?? string.Empty,
                address.PinCode ?? string.Empty,
            }
            .Where(x => !string.IsNullOrEmpty(x)));
    }

    public static string AddressAsText(this AddressDetailDto address)
    {
        return string.Join(", ",
        new string[] {
                address.HouseName ?? string.Empty,
                address.Street ?? string.Empty,
                address.City ?? string.Empty,
                address.State ?? string.Empty,
                address.Country ?? string.Empty,
                address.PinCode ?? string.Empty,
            }
            .Where(x => !string.IsNullOrEmpty(x)));
    }
}
