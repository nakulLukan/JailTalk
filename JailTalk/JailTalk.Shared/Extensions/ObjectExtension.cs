namespace JailTalk.Shared.Extensions;
public static class OObjectExtension
{
    public static T DeepClone<T>(this T data)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(System.Text.Json.JsonSerializer.Serialize(data));
    }
}