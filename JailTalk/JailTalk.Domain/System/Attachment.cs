namespace JailTalk.Domain.System;
public class Attachment
{
    public int Id { get; set; }
    public byte[] Data { get; set; }

    /// <summary>
    /// Relative path without file name
    /// </summary>
    public string RelativeFilePath { get; set; }
    public bool IsBlob { get; set; }
    public string FileName { get; set; }
    public long? FileSizeInBytes { get; set; }
}
