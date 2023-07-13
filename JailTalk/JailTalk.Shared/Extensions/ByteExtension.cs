namespace JailTalk.Shared.Extensions;
public static class ByteExtension
{
    public static string ConvertByteArrayToImgSrc(this byte[] data)
    {
        return $"data:image/jpg;base64,{Convert.ToBase64String(data)}";
    }
}