using JailTalk.Application.Dto.Identity;
using JailTalk.Shared.Models;

namespace JailTalk.Application.Contracts.UserManagement;

public interface IAuthenticationService
{
    public Task<UserSignInResultDto> SignInUser(string email, string password);
    public Task SignOut();
    public Task<List<RolesListDto>> GetAllRoles();
    public Task<string> AddUser(AddUserAccountDto accountDetails);
    public Task<bool> LockUserAccount(string userId, bool lockAccount);
}
