namespace JailTalk.Application.Contracts.Audio;

public interface IAudioService
{
    public bool IsValidAudioFile(byte[] data);
}
