namespace JailTalk.Application.Contracts.AI;

public interface IAppFaceRecognition
{
    bool FaceMatching(byte[] image);
}
