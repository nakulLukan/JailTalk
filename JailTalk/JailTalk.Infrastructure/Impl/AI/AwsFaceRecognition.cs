using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using JailTalk.Application.Contracts.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JailTalk.Infrastructure.Impl.AI;

public class AwsFaceRecognition : IAppFaceRecognition
{
    readonly AmazonRekognitionClient client;
    readonly ILogger<AwsFaceRecognition> logger;

    public AwsFaceRecognition(IConfiguration configuration, ILogger<AwsFaceRecognition> logger)
    {
        string accessKey = configuration["Aws:Rekognition:AccessKey"];
        string secretKey = configuration["Aws:Rekognition:SecretKey"];
        string region = configuration["Aws:Rekognition:Region"];
        var credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
        client = new AmazonRekognitionClient(credentials, Amazon.RegionEndpoint.GetBySystemName(region));
        this.logger = logger;
    }

    public double[] GetFaceEncodings(byte[] image)
    {
        throw new NotImplementedException();
    }

    public bool IsFaceMatching(double[] knownEncoding, double[] unknownEncoding)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsFaceMatching(byte[] knownImage, byte[] unknownImage)
    {
        var request = new CompareFacesRequest
        {
            SourceImage = new Image()
            {
                Bytes = new MemoryStream(knownImage)
            },
            TargetImage = new Image()
            {
                Bytes = new MemoryStream(unknownImage)
            },
        };

        var response = await client.CompareFacesAsync(request);
        if (response.FaceMatches.Count == 0)
        {
            logger.LogError("No faces found");
            return false;
        }
        if (response.FaceMatches.Count > 1)
        {
            logger.LogError("More than 1 face found.");
            return false;
        }
        var confidence = response.FaceMatches[0].Similarity;
        logger.LogInformation("Face match confidence: {confidence}", confidence);
        return confidence > 95F;
    }
}
