using JailTalk.Shared.Models;

namespace JailTalk.Application.Contracts.UserManagement;

public interface IAuthenticationService
{
    public Task<UserSignInResultDto> SignInUser(string username, string password);
}
