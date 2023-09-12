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
    readonly string _bucketName;

    public AwsFaceRecognition(IConfiguration configuration, ILogger<AwsFaceRecognition> logger)
    {
        this.logger = logger;
        client = InitialiseAwsClient(configuration);
        _bucketName = configuration["Aws:S3:BucketName"];
    }

    AmazonRekognitionClient InitialiseAwsClient(IConfiguration configuration)
    {
        string accessKey = configuration["Aws:Rekognition:AccessKey"];
        string secretKey = configuration["Aws:Rekognition:SecretKey"];
        string region = configuration["Aws:Rekognition:Region"];
        bool useBasicCredentialAuthentication = bool.Parse(configuration["Aws:Rekognition:UseBasicCredentialAuthentication"]);
        if (useBasicCredentialAuthentication)
        {
            var credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
            return new AmazonRekognitionClient(credentials, Amazon.RegionEndpoint.GetBySystemName(region));
        }
        else
        {
            return new AmazonRekognitionClient(Amazon.RegionEndpoint.GetBySystemName(region));
        }
    }

    public double[] GetFaceEncodings(byte[] image)
    {
        throw new NotImplementedException();
    }

    public bool IsFaceMatching(double[] knownEncoding, double[] unknownEncoding)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsFaceMatching(string knownFaceRelativePath, byte[] unknownEncoding)
    {
        var request = GetRequest(null, knownFaceRelativePath, unknownEncoding);
        return await Compare(request);
    }

    public async Task<bool> IsFaceMatching(byte[] knownImage, byte[] unknownImage)
    {
        var request = GetRequest(knownImage, null, unknownImage);
        return await Compare(request);
    }

    private async Task<bool> Compare(CompareFacesRequest request)
    {
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

    private CompareFacesRequest GetRequest(byte[] knownFaceData, string knownFaceRelativePath, byte[] unknownFaceData)
    {
        if (knownFaceData == null)
        {
            return new CompareFacesRequest
            {
                SourceImage = new Image()
                {
                    S3Object = new()
                    {
                        Bucket = _bucketName,
                        Name = knownFaceRelativePath,
                    }
                },
                TargetImage = new Image()
                {
                    Bytes = new MemoryStream(unknownFaceData)
                },
            };
        }
        else
        {
            return new CompareFacesRequest
            {
                SourceImage = new Image()
                {
                    Bytes = new MemoryStream(knownFaceData)
                },
                TargetImage = new Image()
                {
                    Bytes = new MemoryStream(unknownFaceData)
                },
            };
        }
    }
}
