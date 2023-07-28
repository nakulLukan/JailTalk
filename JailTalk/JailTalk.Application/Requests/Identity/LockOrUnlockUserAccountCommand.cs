using JailTalk.Application.Contracts.UserManagement;
using JailTalk.Shared.Models;
using MediatR;

namespace JailTalk.Application.Requests.Identity;

public class LockOrUnlockUserAccountCommand : IRequest<ResponseDto<bool>>
{
    public string UserAccountId { get; set; }
    public bool LockAccount { get; set; }
}

public class LockOrUnlockUserAccountCommandHandler : IRequestHandler<LockOrUnlockUserAccountCommand, ResponseDto<bool>>
{
    readonly IAuthenticationService _authenticationService;

    public LockOrUnlockUserAccountCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<ResponseDto<bool>> Handle(LockOrUnlockUserAccountCommand request, CancellationToken cancellationToken)
    {
        _ = await _authenticationService.LockUserAccount(request.UserAccountId, request.LockAccount);
        return new ResponseDto<bool>(true);
    }
}

