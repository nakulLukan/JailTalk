namespace JailTalk.Application.Contracts.Graphics;

public interface IAppQRCodeGenerator
{
    public byte[] GenerateQrCode(string secretMessage);
}