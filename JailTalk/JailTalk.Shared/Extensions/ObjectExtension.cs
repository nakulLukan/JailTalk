using System.Security.Cryptography;

namespace JailTalk.Shared.Extensions;
public static class OObjectExtension
{
    public static T DeepClone<T>(this T data)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(System.Text.Json.JsonSerializer.Serialize(data));
    }

    public static string ToHash(this Guid guid)
    {
        // Convert the GUID to bytes
        byte[] guidBytes = guid.ToByteArray();

        // Create a SHA-256 hash object
        using (SHA256 sha256 = SHA256.Create())
        {
            // Compute the hash
            byte[] hashBytes = sha256.ComputeHash(guidBytes);

            // Convert the hash to a hexadecimal string
            string hashHex = BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
            return hashHex;
        }
    }
}