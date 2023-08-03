namespace JailTalk.Shared.Constants;

public static class RegularExpressionPatternConstant
{
    public const string DeviceCode = "^[A-Za-z0-9\\-]+$";
    public const string CountryCode = "^[0-9]{0,5}$";
    public const string PhoneNumber = "^[0-9]{0,15}$";
    public const string MacAddress = "^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$";
    public const string RoleName = "^[a-z-]*$";
    public const string IntegerOnly = "^[0-9]+$";
    public const string FloatOnly = "^[-+]?[0-9]*\\.?[0-9]+$";
}
