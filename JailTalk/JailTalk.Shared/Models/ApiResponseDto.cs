namespace JailTalk.Shared.Models;

public class ApiResponseDto<T>
    where T : class
{
    public T Data { get; set; }
    public ApiResponseDto(T data)
    {
        Data = data;
    }
}
