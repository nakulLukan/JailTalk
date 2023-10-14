using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Email;
using JailTalk.Application.Contracts.Graphics;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Contracts.Storage;
using JailTalk.Application.Services;
using JailTalk.Domain.Prison;
using JailTalk.Shared;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Web;

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
    readonly IAppQRCodeGenerator _appQrCodeGenerator;
    readonly IFileStorage _fileStorage;
    readonly IConfiguration _configuration;

    public RequestJailAccountRechargeCommandHandler(
        IAppDbContext dbContext,
        IAppRequestContext requestContext,
        IEmailService emailService,
        IAppQRCodeGenerator appQrCodeGenerator,
        IFileStorage fileStorage,
        IConfiguration configuration)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
        _emailService = emailService;
        _appQrCodeGenerator = appQrCodeGenerator;
        _fileStorage = fileStorage;
        _configuration = configuration;
    }

    public async Task<long> Handle(RequestJailAccountRechargeCommand request, CancellationToken cancellationToken)
    {
        var associatedPrisonId = _requestContext.GetAssociatedPrisonId();
        var secretGuid = Guid.NewGuid();
        var jailDetails = await _dbContext.Jails
            .WhereInPrison(x => x.Id, associatedPrisonId)
            .Where(x => x.Id == request.JailId)
            .Select(x => new
            {
                x.Code,
                x.Name,
            })
            .FirstOrDefaultAsync(cancellationToken) ?? throw new AppException(CommonExceptionMessages.PermissionDenied);
        var requestedUser = await _requestContext.GetUserId();
        var requestedBy = await _dbContext.Users.Where(x => x.Id == requestedUser)
                .Select(x => new
                {
                    x.FullName,
                    x.Email
                })
                .FirstAsync(cancellationToken);
        var rechargeRequest = new JailAccountRechargeRequest
        {
            RechargeAmount = request.RechargeAmount,
            JailId = request.JailId,
            RequestedBy = requestedUser,
            RequestedOn = AppDateTime.UtcNow,
            RequestStatus = Shared.JailAccountRechargeRequestStatus.Pending,
            RetryCount = 0,
            RechargeSecretHash = secretGuid.ToHash(),
            ExpiresOn = AppDateTime.UtcNow.AddDays(3),
        };
        _dbContext.JailAccountRechargeRequests.Add(rechargeRequest);
        await _dbContext.SaveAsync(cancellationToken);
        var secretMessage = $"RequestId={HttpUtility.UrlEncode(rechargeRequest.Id.ToString())}&" +
            $"RechargeSecret={HttpUtility.UrlEncode(secretGuid.ToString())}&" +
            $"Amount={HttpUtility.UrlEncode(request.RechargeAmount.ToString())}&" +
            $"PrisonCode={HttpUtility.UrlEncode(jailDetails.Code)}&" +
            $"PrisonName={HttpUtility.UrlEncode(jailDetails.Name)}&" +
            $"RequestedUserFullName={HttpUtility.UrlEncode(requestedBy.FullName)}&" +
            $"RequestedUserEmail={HttpUtility.UrlEncode(requestedBy.Email)}";

        //var qrCode = _appQrCodeGenerator.GenerateQrCode(secretMessage);
        //var qrCodeFile = await _fileStorage.UploadFile(qrCode, rechargeRequest.Id.ToString(), AttachmentHelper.RechargeRequestTempPath, cancellationToken);

        // Get email template and its body parameters
        var init = EmailContentInitialiser.RechargeRequestBuilder(secretMessage);
        var emailBody = await _emailService.GenerateBody(init.TemplateName, init.BodyParams);

        string requestRecieverEmail = _configuration["AccountManager:RechargeRequestRecieverEmailAddress"];
        await _emailService.SendEmailAsync(requestRecieverEmail, $"Recharge Request #{rechargeRequest.Id}", emailBody);
        return rechargeRequest.Id;
    }
}

