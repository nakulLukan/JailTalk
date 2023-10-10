using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Email;
using JailTalk.Application.Contracts.Http;
using JailTalk.Domain.Prison;
using JailTalk.Shared.Utilities;
using MediatR;

namespace JailTalk.Application.Requests.Jail;

public class RequestJailAccountRechargeCommand : IRequest<long>
{
    public float RechargeAmount { get; set; }
    public int JailId { get; set; }
}
public class RequestJailAccountRechargeCommandHandler : IRequestHandler<RequestJailAccountRechargeCommand, long>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;
    readonly IEmailService _emailService;

    public RequestJailAccountRechargeCommandHandler(IAppDbContext dbContext, IAppRequestContext requestContext, IEmailService emailService)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
        _emailService = emailService;
    }

    public async Task<long> Handle(RequestJailAccountRechargeCommand request, CancellationToken cancellationToken)
    {
        var rechargeRequest = new JailAccountRechargeRequest
        {
            RechargeAmount = request.RechargeAmount,
            JailId = request.JailId,
            RequestedBy = await _requestContext.GetUserId(),
            RequestedOn = AppDateTime.UtcNow,
            RequestStatus = Shared.JailAccountRechargeRequestStatus.Pending,
            RetryCount = 0,
            RechargeSecretHash = Guid.NewGuid().ToString(),
            ExpiresOn = AppDateTime.UtcNow.AddDays(3),
        };
        _dbContext.JailAccountRechargeRequests.Add(rechargeRequest);
        await _dbContext.SaveAsync(cancellationToken);
        return rechargeRequest.Id;
    }
}

