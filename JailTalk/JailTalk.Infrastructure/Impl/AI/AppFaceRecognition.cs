using FaceRecognitionDotNet;
using JailTalk.Application.Contracts.AI;
using JailTalk.Shared.Utilities;
using System.Drawing;

namespace JailTalk.Infrastructure.Impl.AI;

public class AppFaceRecognition : IAppFaceRecognition
{
    readonly FaceRecognition _faceRecognition;

    public AppFaceRecognition()
    {
        _faceRecognition = FaceRecognition.Create($"{AppDomain.CurrentDomain.BaseDirectory}/Data/MLModels/FaceRecognition");
    }

    public bool IsFaceMatching(double[] knownEncoding, double[] unknownEncoding)
    {
        FaceEncoding knownFaceEncoding = FaceRecognition.LoadFaceEncoding(knownEncoding);
        FaceEncoding unknownFaceEncoding = FaceRecognition.LoadFaceEncoding(unknownEncoding);
        return FaceRecognition.CompareFace(knownFaceEncoding, unknownFaceEncoding);
    }

    public double[] GetFaceEncodings(byte[] image)
    {
        Bitmap bmp;
        using (var ms = new MemoryStream(image))
        {
            bmp = new Bitmap(ms);
            var encodings = _faceRecognition.FaceEncodings(FaceRecognition.LoadImage(bmp));
            if (!encodings.Any())
            {
                throw new AppException("No detectable face found");
            }

            if (encodings.Count() > 1)
            {
                throw new AppException("More than 1 face found.");
            }

            return encodings.First().GetRawEncoding();
        }
    }

    public Task<bool> IsFaceMatching(byte[] knownImage, byte[] unknownImage)
    {
        throw new NotImplementedException();
    }
}
