namespace JailTalk.Shared.Models;

public class BrowserFileDto : IDisposable
{
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }
    public DateTimeOffset LastModified { get; set; }
    public MemoryStream DataStream { get; set; }

    public void Dispose()
    {
        DataStream?.Dispose();
    }
}
