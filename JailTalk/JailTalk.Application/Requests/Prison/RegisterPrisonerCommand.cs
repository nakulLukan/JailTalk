using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Dto.Prison;
using JailTalk.Domain.Prison;
using JailTalk.Shared;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Prison;

public class RegisterPrisonerCommand : NewPrisonerDto, IRequest<ResponseDto<Guid>>
{
}

public class RegisterPrisonerCommandHandler : IRequestHandler<RegisterPrisonerCommand, ResponseDto<Guid>>
{
    readonly IAppDbContext _dbContext;

    public RegisterPrisonerCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResponseDto<Guid>> Handle(RegisterPrisonerCommand request, CancellationToken cancellationToken)
    {
        if (await _dbContext.Prisoners.AnyAsync(x => x.Pid.ToLower() == request.Pid.ToLower()))
        {
            throw new AppException("A prisoner with same 'PID' exists.");
        }

        Prisoner prisoner = new Prisoner()
        {
            JailId = request.JailId.Value,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            MiddleName = request.MiddleName.Trim(),
            FullName = string.Join(' ', new string[] { request.FirstName.Trim(), request.MiddleName.Trim(), request.LastName.Trim() }),
            Pid = request.Pid.Trim(),
            CreatedOn = AppDateTime.UtcNow,
            UpdatedOn = AppDateTime.UtcNow,
            CreatedBy = string.Empty,
            UpdatedBy = string.Empty,
            Gender = (Gender)request.Gender,
            PrisonerFunction = new()
        };

        _dbContext.Prisoners.Add(prisoner);
        await _dbContext.SaveAsync(cancellationToken);

        return new ResponseDto<Guid>(prisoner.Id);
    }
}

