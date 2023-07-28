using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.UserManagement;
using JailTalk.Application.Dto.Identity;
using JailTalk.Shared;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Identity;

public class AddUserAccountCommand : AddUserAccountDto, IRequest<ResponseDto<string>>
{
}

public class AddUserAccountCommandHandler : IRequestHandler<AddUserAccountCommand, ResponseDto<string>>
{
    readonly IAppDbContext _dbContext;
    readonly IAuthenticationService _authenticationService;

    public AddUserAccountCommandHandler(IAppDbContext dbContext, IAuthenticationService authenticationService)
    {
        _dbContext = dbContext;
        _authenticationService = authenticationService;
    }

    public async Task<ResponseDto<string>> Handle(AddUserAccountCommand request, CancellationToken cancellationToken)
    {
        await ValidateUserNameAndEmailAddress(request, cancellationToken);
        var userId = await _authenticationService.AddUser(request);
        return new ResponseDto<string>(userId);
    }

    private async Task ValidateUserNameAndEmailAddress(AddUserAccountCommand request, CancellationToken cancellationToken)
    {
        var userDetails = await _dbContext.Users
                    .Where(x => x.NormalizedUserName == request.Username.Normalized()
                        || x.NormalizedEmail == request.Email.Normalized())
                    .Select(x => new
                    {
                        x.NormalizedEmail,
                        x.NormalizedUserName
                    })
                    .ToListAsync(cancellationToken);
        if (userDetails.Any(x => x.NormalizedEmail == request.Email.Normalized()))
        {
            throw new AppException("An user account with same email address is already registered in this system.");
        }
        if (userDetails.Any(x => x.NormalizedUserName == request.Username.Normalized()))
        {
            throw new AppException("An user account with same username is already registered in this system.");
        }

        // If the users role is not super-admin then a prison value should be selected.
        // All user accounts with role other than super-admin must be associated to a prison.
        if (request.RoleName != AppRoleNames.SuperAdmin && !request.PrisonId.HasValue)
        {
            throw new AppException("Prison field is mandatory for the selected user. Please choose a prison.");
        }
    }
}

