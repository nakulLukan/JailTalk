using FluentValidation;
using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Services;
using JailTalk.Domain.Prison;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
namespace JailTalk.Application.Requests.Jail;

public class RechargeJailAccountCommand : IRequest<ResponseDto<string>>
{
    public long RequestId { get; set; }
    public byte[] RechargeSecret { get; set; }
    public bool IsCompleteCommand { get; set; }
}

public class RechargeJailAccountCommandHandler : IRequestHandler<RechargeJailAccountCommand, ResponseDto<string>>
{
    readonly IAppDbContext _dbContext;
    readonly IConfiguration _configuration;
    readonly short _maxFailAttempt = 3;
    readonly ILogger<RechargeJailAccountCommandHandler> _logger;

    public RechargeJailAccountCommandHandler(IAppDbContext dbContext, IConfiguration configuration, ILogger<RechargeJailAccountCommandHandler> logger)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<ResponseDto<string>> Handle(RechargeJailAccountCommand request, CancellationToken cancellationToken)
    {
        await ValidateLicense();
        ValidateSharedSecret(request);
        var rechargeRequest = await _dbContext.JailAccountRechargeRequests.AsTracking()
            .SingleOrDefaultAsync(x => x.Id == request.RequestId, cancellationToken) ?? throw new AppApiException(HttpStatusCode.NotFound, "ERR-2", "Unkown Request");

        await ValidateRequest(request, rechargeRequest, cancellationToken);

        await RechargeAccount(request, rechargeRequest, cancellationToken);

        return new ResponseDto<string>("The request processed successfully");
    }

    private async Task ValidateLicense()
    {
        await ProductLicenseService.ThrowIfLicenseIsInvalid();
    }

    private async Task ValidateRequest(RechargeJailAccountCommand request, JailAccountRechargeRequest rechargeRequest, CancellationToken cancellationToken)
    {
        if (rechargeRequest.RequestStatus != Shared.JailAccountRechargeRequestStatus.Pending)
        {
            throw new AppApiException(HttpStatusCode.BadRequest, "ERR-6", "Request already processed.");
        }
        if (rechargeRequest.RetryCount >= _maxFailAttempt)
        {
            // Update the status only if the status is pending. This is to track the status of previous request.
            if (rechargeRequest.RequestStatus == Shared.JailAccountRechargeRequestStatus.Pending)
            {
                rechargeRequest.RequestCompletedOn = AppDateTime.UtcNow;
                rechargeRequest.RequestStatus = Shared.JailAccountRechargeRequestStatus.FailAttemptExceeded;
                await _dbContext.SaveAsync(cancellationToken);
            }

            throw new AppApiException(HttpStatusCode.BadRequest, "ERR-5", "Too many request. Please make a new recharge request.");
        }

        if (rechargeRequest.ExpiresOn.HasValue && rechargeRequest.ExpiresOn < AppDateTime.UtcNow)
        {
            rechargeRequest.RequestCompletedOn = AppDateTime.UtcNow;
            rechargeRequest.RequestStatus = Shared.JailAccountRechargeRequestStatus.Expired;
            rechargeRequest.RetryCount += 1;
            await _dbContext.SaveAsync(cancellationToken);
            throw new AppApiException(HttpStatusCode.BadRequest, "ERR-4", "The request has expired.");
        }

        await ValidateRechargeSecretHash(rechargeRequest, request.RechargeSecret, cancellationToken);
    }

    private async Task ValidateRechargeSecretHash(JailAccountRechargeRequest rechargeRequest, byte[] sharedSecret, CancellationToken cancellationToken)
    {
        var secret = _configuration["AccountManager:RechargeSecret"];
        int sharedSecretDynamicComponent = (int)(AppDateTime.UtcNow - DateTime.UnixEpoch).TotalMinutes;
        string[] possibleSharedSecret = new string[]
        {
            $"{secret}:{sharedSecretDynamicComponent}",
            $"{secret}:{sharedSecretDynamicComponent + 1}",
            $"{secret}:{sharedSecretDynamicComponent - 1}",
        };

        bool secretMatched = false;
        foreach (var possibleSecret in possibleSharedSecret)
        {
            if (Guid.TryParse(CryptoEngine.DecryptText(sharedSecret, possibleSecret), out Guid guid) && rechargeRequest.RechargeSecretHash == guid.ToHash())
            {
                secretMatched = true;
            }
        }

        if (!secretMatched)
        {
            rechargeRequest.RetryCount += 1;
            await _dbContext.SaveAsync(cancellationToken);
            throw new AppApiException(HttpStatusCode.BadRequest, "ERR-3", "The request failed to process. Invalid secret.");
        }
    }

    private void ValidateSharedSecret(RechargeJailAccountCommand request)
    {
        var secret = _configuration["AccountManager:RechargeSecret"];
        int sharedSecretDynamicComponent = (int)(AppDateTime.UtcNow - DateTime.UnixEpoch).TotalMinutes;
        string[] possibleSharedSecret = new string[]
        {
            $"{secret}:{sharedSecretDynamicComponent}",
            $"{secret}:{sharedSecretDynamicComponent + 1}",
            $"{secret}:{sharedSecretDynamicComponent - 1}",
        };

        if (!possibleSharedSecret.Any(x => Guid.TryParse(request.RechargeSecret.DecryptText(x), out Guid _)))
        {
            throw new AppApiException(HttpStatusCode.Unauthorized, "ERR-1", "Shared key is not accepted.");
        }
    }

    private async Task RechargeAccount(RechargeJailAccountCommand request, JailAccountRechargeRequest rechargeRequest, CancellationToken cancellationToken)
    {
        var accountBalance = await _dbContext.JailAccountBalance.AsTracking()
            .SingleOrDefaultAsync(x => x.JailId == rechargeRequest.JailId, cancellationToken);
        // If request is to decline then do not recharge. Just decline the request
        if (request.IsCompleteCommand)
        {
            if (accountBalance == null)
            {
                accountBalance = new Domain.Prison.JailAccountBalance
                {
                    BalanceAmount = rechargeRequest.RechargeAmount,
                    JailId = rechargeRequest.JailId
                };

                _dbContext.JailAccountBalance.Add(accountBalance);
            }
            else
            {
                accountBalance.BalanceAmount += rechargeRequest.RechargeAmount;
            }
        }

        rechargeRequest.RequestStatus = request.IsCompleteCommand ? Shared.JailAccountRechargeRequestStatus.Completed : Shared.JailAccountRechargeRequestStatus.Declined;
        rechargeRequest.RequestCompletedOn = AppDateTime.UtcNow;

        await _dbContext.SaveAsync(cancellationToken);
    }
}

public class RechargeJailAccountCommandValidator : AbstractValidator<RechargeJailAccountCommand>
{
    public RechargeJailAccountCommandValidator()
    {
        RuleFor(x => x.RechargeSecret)
            .NotNull()
            .NotEmpty()
                .WithMessage("Invalid shared secret.");
    }
}


