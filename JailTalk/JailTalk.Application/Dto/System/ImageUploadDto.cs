namespace JailTalk.Application.Dto.System;

public class ImageUploadDto : AttachmentBaseDto
{
    public Stream DataStream { get; set; }
    public string ContentType { get; set; }
}
