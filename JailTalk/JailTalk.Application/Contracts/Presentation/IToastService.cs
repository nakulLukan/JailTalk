namespace JailTalk.Application.Contracts.Presentation;

public interface IToastService
{
    void Success(string content, int duration = 3);
    void Error(string content, int duration = 3);
}

