using JailTalk.Application.Contracts.UserManagement;
using JailTalk.Application.Dto.Identity;
using JailTalk.Shared.Models;
using MediatR;

namespace JailTalk.Application.Requests.Identity;

public class RolesListQuery : IRequest<ResponseDto<List<RolesListDto>>>
{
}

public class RolesListQueryHandler : IRequestHandler<RolesListQuery, ResponseDto<List<RolesListDto>>>
{
    readonly IAuthenticationService _authService;

    public RolesListQueryHandler(IAuthenticationService authService)
    {
        _authService = authService;
    }

    public async Task<ResponseDto<List<RolesListDto>>> Handle(RolesListQuery request, CancellationToken cancellationToken)
    {
        var roles = await _authService.GetAllRoles();
        int index = 1;
        roles.ForEach(x => x.Serial = index++);
        return new(roles);
    }
}

