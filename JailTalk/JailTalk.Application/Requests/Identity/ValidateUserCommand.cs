using JailTalk.Application.Contracts.UserManagement;
using JailTalk.Application.Dto.Identity;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
namespace JailTalk.Application.Requests.Identity;

public class ValidateUserCommand : SignInDto, IRequest<UserSignInResultDto>
{
}

public class ValidateUserCommandHandler : IRequestHandler<ValidateUserCommand, UserSignInResultDto>
{
    readonly IAuthenticationService _authenticationService;
    public ValidateUserCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<UserSignInResultDto> Handle(ValidateUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.SignInUser(request.Email, request.Password);
        if (!result.Succeeded)
        {
            throw new AppException("Username or Password does not match");
        }

        return result;
    }
}
