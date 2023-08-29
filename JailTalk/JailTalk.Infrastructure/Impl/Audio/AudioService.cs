using JailTalk.Application.Contracts.Audio;

namespace JailTalk.Infrastructure.Impl.Audio;

public class AudioService : IAudioService
{
    public bool IsValidAudioFile(byte[] data)
    {
        return true;
    }
}
