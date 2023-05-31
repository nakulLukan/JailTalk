using JailTalk.Application.Contracts.UserManagement;
using JailTalk.Application.Dto.Identity;
using JailTalk.Domain.Identity;
using JailTalk.Shared.Utilities;
using MediatR;
namespace JailTalk.Application.Requests.Identity;

public class ValidateUserCommand : SignInDto, IRequest<AppUser>
{
}

public class ValidateUserCommandHandler : IRequestHandler<ValidateUserCommand, AppUser>
{
    readonly IAuthenticationService _authenticationService;
    public ValidateUserCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<AppUser> Handle(ValidateUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.SignInUser(request.Username, request.Password);
        if (!result.Succeeded)
        {
            throw new AppException("Username or Password does not match");
        }

        return null;
    }
}
