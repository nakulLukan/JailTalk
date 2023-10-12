using JailTalk.Application.Contracts.Graphics;
using SkiaSharp;
using ZXing;
using ZXing.QrCode;
using ZXing.SkiaSharp;

namespace JailTalk.Infrastructure.Impl.Graphics;

public class AppQRCodeGenerator : IAppQRCodeGenerator
{
    public byte[] GenerateQrCode(string secretMessage)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Width = 512,
                Height = 512,
            }
        };

        using var qrCodeImage = writer.Write(secretMessage);
        using var skImage = SKImage.FromBitmap(qrCodeImage);
        using var stream = new MemoryStream();
        skImage.Encode().AsStream().CopyTo(stream);
        return stream.ToArray();
    }
}
