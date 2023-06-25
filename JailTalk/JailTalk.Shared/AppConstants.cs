namespace JailTalk.Shared;

public static class AppSettingKeys
{
    public const string JwtSettingsKey = "JwtSettings:Key";
    public const string JwtSettingsIssuer = "JwtSettings:Issuer";
    public const string JwtSettingsAudience = "JwtSettings:Audience";
}

public static class RegularExpressionPattern
{
    public const string DeviceCode = "^[A-Za-z0-9\\-]+$";
    public const string MacAddress = "^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$";
}