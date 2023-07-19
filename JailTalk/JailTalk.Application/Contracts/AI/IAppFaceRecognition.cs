namespace JailTalk.Application.Contracts.AI;

public interface IAppFaceRecognition
{
    double[] GetFaceEncodings(byte[] image);
    bool IsFaceMatching(double[] knownEncoding, double[] unknownEncoding);
    Task<bool> IsFaceMatching(byte[] knownImage, byte[] unknownImage);
}
