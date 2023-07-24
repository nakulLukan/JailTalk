using JailTalk.Application.Contracts.UserManagement;
using JailTalk.Application.Dto.Identity;
using JailTalk.Shared.Models;

namespace JailTalk.Api.Impl.UserManagement;

public class AuthenticationService : IAuthenticationService
{
    public Task<List<RolesListDto>> GetAllRoles()
    {
        throw new NotImplementedException();
    }

    public Task<UserSignInResultDto> SignInUser(string email, string password)
    {
        throw new NotImplementedException();
    }
}
