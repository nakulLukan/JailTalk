using FaceRecognitionDotNet;

namespace JailTalk.Infrastructure.Impl.AI;

public class AppFaceRecognition
{
    readonly FaceRecognition _faceRecognition;

    public AppFaceRecognition()
    {
        _faceRecognition = FaceRecognition.Create($"{Directory.GetCurrentDirectory()}/Data/MLModels/FaceRecognition");
    }
}
