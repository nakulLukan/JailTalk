namespace JailTalk.Application.Contracts.Storage;

public interface IFileStorage
{
    /// <summary>
    /// Uploads the file to the storage.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="fileName"></param>
    /// <param name="filePath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<(string SignedUrl, string RelativePath)> UploadFile(
        byte[] file,
        string fileName,
        string filePath,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets a signed url with low lifetime.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public string GetPresignedUrl(string fileName, string filePath);
}
