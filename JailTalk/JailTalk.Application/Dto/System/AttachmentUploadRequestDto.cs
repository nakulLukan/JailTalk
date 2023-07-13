namespace JailTalk.Application.Dto.System;

public class AttachmentUploadRequestDto
{
    public string FileName { get; set; }
    public string FileContent { get; set; }
    public byte[] Data { get; set; }
    public bool SaveAsThumbnail { get; set; }
}
