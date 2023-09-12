using Amazon.S3;
using Amazon.S3.Model;
using JailTalk.Application.Contracts.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JailTalk.Infrastructure.Impl.Storage;

public class AwsStorage : IFileStorage
{
    readonly ILogger<AwsStorage> _logger;
    AmazonS3Client _client;
    string _bucketName;

    public AwsStorage(IConfiguration configuration, ILogger<AwsStorage> logger)
    {
        _client = InitialiseAwsClient(configuration);
        _bucketName = configuration["Aws:S3:BucketName"];
        _logger = logger;
    }

    /// <summary>
    /// Uploads the file to the storage.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="fileName"></param>
    /// <param name="filePath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<(string SignedUrl, string RelativePath)> UploadFile(
        byte[] file,
        string fileName,
        string filePath,
        CancellationToken cancellationToken)
    {
        PutObjectRequest putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = RelativePath(filePath, fileName),
            InputStream = new MemoryStream(file), // Convert byte array to stream
        };

        PutObjectResponse response = await _client.PutObjectAsync(putRequest, cancellationToken);
        _logger.LogInformation("Filename: {filename}, File upload status {status}", RelativePath(filePath, fileName), response.HttpStatusCode);
        if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
        {
            _logger.LogError("Failed to upload file. Reason: {reason}", response.ToString());
        }
        string signedUrl = GetPresignedUrl(fileName, filePath);
        return (signedUrl, RelativePath(filePath, fileName));
    }

    /// <summary>
    /// Gets a signed url with low lifetime.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public string GetPresignedUrl(string fileName, string filePath)
    {
        GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = RelativePath(filePath, fileName),
            Expires = DateTime.Now.AddMinutes(30) // Set expiration time for the URL
        };

        string signedUrl = _client.GetPreSignedURL(request);
        return signedUrl;
    }

    /// <summary>
    /// Gets a signed url with low lifetime.
    /// </summary>
    /// <param name="fileRelativePath"></param>
    /// <returns></returns>
    public string GetPresignedUrl(string fileRelativePath)
    {
        if (string.IsNullOrEmpty(fileRelativePath))
        {
            return string.Empty;
        }

        GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = fileRelativePath,
            Expires = DateTime.Now.AddMinutes(30) // Set expiration time for the URL
        };

        string signedUrl = _client.GetPreSignedURL(request);
        return signedUrl;
    }

    public async Task<bool> DeleteFile(string fullFilePath)
    {
        var response = await _client.DeleteObjectAsync(new()
        {
            BucketName = _bucketName,
            Key = fullFilePath,
        });

        return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
    }

    private AmazonS3Client InitialiseAwsClient(IConfiguration configuration)
    {
        string accessKey = configuration["Aws:S3:AccessKey"];
        string secretKey = configuration["Aws:S3:SecretKey"];
        string region = configuration["Aws:S3:Region"];
        bool useBasicCredentialAuthentication = bool.Parse(configuration["Aws:S3:UseBasicCredentialAuthentication"]);
        if (useBasicCredentialAuthentication)
        {
            var credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
            return new AmazonS3Client(credentials, Amazon.RegionEndpoint.GetBySystemName(region));
        }
        else
        {
            return new AmazonS3Client(Amazon.RegionEndpoint.GetBySystemName(region));
        }
    }

    private static string RelativePath(string path, string fileName)
    {
        if (string.IsNullOrEmpty(path))
        {
            return fileName;
        }

        return path + "/" + fileName;
    }
}
