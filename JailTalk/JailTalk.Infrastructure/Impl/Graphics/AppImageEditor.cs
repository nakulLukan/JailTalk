using JailTalk.Application.Contracts.Graphics;
using SkiaSharp;

namespace JailTalk.Infrastructure.Impl.Graphics;

public class AppImageEditor : IAppImageEditor
{
    public byte[] ConvertImageToThumbnail(byte[] imageBytes, int width, int height)
    {
        using (var stream = new MemoryStream(imageBytes))
        using (var skStream = new SKManagedStream(stream))
        using (var originalBitmap = SKBitmap.Decode(skStream))
        using (var thumbnailBitmap = originalBitmap.Resize(new SKImageInfo(width, height), SKFilterQuality.High))
        using (var image = SKImage.FromBitmap(thumbnailBitmap))
        using (var encodedData = image.Encode(SKEncodedImageFormat.Jpeg, 100)) // Adjust the image quality as needed
        {
            using (var outputStream = new MemoryStream())
            {
                encodedData.SaveTo(outputStream);
                return outputStream.ToArray();
            }
        }
    }
}
