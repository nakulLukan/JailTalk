namespace JailTalk.Application.Dto.Prison;

public class PrisonerCallRecordingResponseDto
{
    public string SignedUrl { get; set; }
    public string FileName { get; set; }
    public string CallDurationAsText { get; set; }
    public string FileSizeAsText { get; set; }
}
