namespace JailTalk.Application.Contracts.Graphics;

public interface IAppImageEditor
{
    public byte[] ConvertImageToThumbnail(byte[] imageBytes, int width, int height);
}
