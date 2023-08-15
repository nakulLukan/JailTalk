using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.Jail;
using JailTalk.Application.Services;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.Jail;

public class AddPrisonCommand : AddPrisonDto, IRequest<ResponseDto<int>>
{
}

public class AddPrisonCommandHandler : IRequestHandler<AddPrisonCommand, ResponseDto<int>>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;

    public AddPrisonCommandHandler(IAppDbContext dbContext, IAppRequestContext requestContext)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
    }

    public async Task<ResponseDto<int>> Handle(AddPrisonCommand request, CancellationToken cancellationToken)
    {
        var duplicatePrisonDetails = await _dbContext.Jails
            .Where(x => x.Code.ToLower() == request.PrisonCode.ToLower()
                || x.Name.ToLower() == request.PrisonName.ToLower())
            .Select(x => new
            {
                x.Name,
                x.Code
            })
            .ToListAsync(cancellationToken);

        if (duplicatePrisonDetails.Any(x => x.Name.Equals(request.PrisonName.ToLower(), StringComparison.InvariantCultureIgnoreCase)))
        {
            throw new AppException("Prison Name cannot be same. Another prison with same name exists.");
        }
        if (duplicatePrisonDetails.Any(x => x.Code.Equals(request.PrisonCode.ToLower(), StringComparison.InvariantCultureIgnoreCase)))
        {
            throw new AppException("Prison Code cannot be same. Another prison with same code exists.");
        }

        var userId = await _requestContext.GetUserId();
        var prison = new Domain.Prison.Jail
        {
            Name = request.PrisonName,
            Code = request.PrisonCode,
            Address = AddressHelper.ToNewAddress(request.Address),
            CreatedOn = AppDateTime.UtcNow,
            UpdatedOn = AppDateTime.UtcNow,
            CreatedBy = userId,
            UpdatedBy = userId,
        };

        _dbContext.Jails.Add(prison);
        await _dbContext.SaveAsync(cancellationToken);
        return new(prison.Id);
    }
}
