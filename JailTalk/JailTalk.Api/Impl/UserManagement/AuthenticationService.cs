using JailTalk.Application.Contracts.UserManagement;
using JailTalk.Shared.Models;

namespace JailTalk.Api.Impl.UserManagement;

public class AuthenticationService : IAuthenticationService
{
    public Task<UserSignInResultDto> SignInUser(string email, string password)
    {
        throw new NotImplementedException();
    }
}
