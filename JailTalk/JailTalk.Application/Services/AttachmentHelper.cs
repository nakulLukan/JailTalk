namespace JailTalk.Application.Services;

public class AttachmentHelper
{
    public static string GenerateFullPath(string basePath, string fileName)
    {
        if (string.IsNullOrEmpty(basePath) || string.IsNullOrEmpty(fileName))
        {
            return string.Empty;
        }

        return $"{basePath}/{fileName}";
    }

    public static string TempFolder => "temp";
    public static string RechargeRequestTempPath => TempFolder + "/rechage-requests";
}
