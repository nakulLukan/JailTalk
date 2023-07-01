namespace JailTalk.Shared;

public static class AppSettingKeys
{
    public const string JwtSettingsKey = "JwtSettings:Key";
    public const string JwtSettingsIssuer = "JwtSettings:Issuer";
    public const string JwtSettingsAudience = "JwtSettings:Audience";
    public const string JwtSettingsSessionVerificationKey = "JwtSettings:SessionVerificationKey";
}

public static class RegularExpressionPattern
{
    public const string DeviceCode = "^[A-Za-z0-9\\-]+$";
    public const string CountryCode = "^[0-9]{0,5}$";
    public const string PhoneNumber = "^[0-9]{0,15}$";
    public const string MacAddress = "^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$";
}

public static class AppClaims
{
    public const string PrisonId = "PrisonId";
}

public static class HttpHeader
{
    public const string SessionToken = "SessionToken";
}