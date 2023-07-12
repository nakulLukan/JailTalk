namespace JailTalk.Domain.System;
public class Attachment
{
    public int Id { get; set; }
    public byte[] Data { get; set; }
    public string Path { get; set; }
    public bool IsBlob { get; set; }
    public string FileName { get; set; }
}
