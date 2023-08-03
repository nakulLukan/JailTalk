using FluentValidation;
using JailTalk.Application.Contracts.AI;
using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Shared;
using JailTalk.Shared.Constants;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace JailTalk.Application.Requests.Identity;

public class PrisonerTokenQuery : IRequest<ResponseDto<string>>
{
    public byte[] FaceImageData { get; set; }
    public string Pid { get; set; }
}

public class PrisonerTokenQueryHandler : IRequestHandler<PrisonerTokenQuery, ResponseDto<string>>
{
    readonly IConfiguration _configuration;
    readonly IAppDbContext _dbContext;
    readonly IDeviceRequestContext _requestContext;
    readonly ILogger<PrisonerTokenQueryHandler> _logger;
    readonly IAppFaceRecognition _faceRecognition;

    record PrisonerDto(Guid Id, bool IsBlocked, bool IsActive, string FullName, int PrisonId, List<double[]> FaceEncodings, List<byte[]> FaceImages);

    public PrisonerTokenQueryHandler(IConfiguration configuration, IAppDbContext dbContext, IDeviceRequestContext requestContext, ILogger<PrisonerTokenQueryHandler> logger, IAppFaceRecognition faceRecognition)
    {
        _configuration = configuration;
        _dbContext = dbContext;
        _requestContext = requestContext;
        _logger = logger;
        _faceRecognition = faceRecognition;
    }

    public async Task<ResponseDto<string>> Handle(PrisonerTokenQuery request, CancellationToken cancellationToken)
    {
        var prisoner = await _dbContext.Prisoners
            .Where(x => x.Pid == request.Pid)
            .Select(x => new PrisonerDto(
                x.Id,
                x.IsBlocked,
                x.IsActive,
                x.FullName,
                x.JailId,
                x.FaceEncodings.Select(y => y.FaceEncoding.Encoding).ToList(),
                x.FaceEncodings.Select(y => y.Attachment.Data).ToList()))
            .FirstOrDefaultAsync(cancellationToken);
        if (prisoner is null)
        {
            throw new AppApiException(HttpStatusCode.Unauthorized, "PT-0001", "PID does not exists.");
        }
        if (prisoner.IsBlocked)
        {
            _logger.LogError("The prisoner is blocked.");
            throw new AppApiException(HttpStatusCode.Forbidden, "PT-0002", "Inactive record");
        }

        var prisonerId = _requestContext.GetJailId();
        if (prisonerId != prisoner.PrisonId)
        {
            _logger.LogError("Device trying to access prisoner in another jail.");
            throw new AppApiException(HttpStatusCode.Forbidden, "PT-0003", "Unauthorized access");
        }

        if (bool.Parse(_configuration[AppSettingKeysConstant.JwtValidateFaceImage]))
        {
            await AuthenticateByFaceId(request, prisoner);
        }

        var token = CreateToken(prisoner);
        return new ResponseDto<string>(token);
    }

    private async Task AuthenticateByFaceId(PrisonerTokenQuery request, PrisonerDto prisoner)
    {
        _logger.LogInformation("About to match face details of prisoner #{pid}", request.Pid);
        var hasMatch = false;
        List<Task<bool>> results = new();
        foreach (var existingFace in prisoner.FaceImages)
        {
            results.Add(_faceRecognition.IsFaceMatching(existingFace, request.FaceImageData));
        }

        await Task.WhenAll(results);
        _logger.LogInformation("Face match results completed");

        hasMatch = results.All(x => x.Result);
        if (!hasMatch)
        {
            throw new AppApiException(HttpStatusCode.Unauthorized, "PT-0004", "Face Id does not match");
        }
    }

    private string CreateToken(PrisonerDto device)
    {
        IdentityModelEventSource.ShowPII = false;
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration[AppSettingKeysConstant.JwtSettingsSessionVerificationKey]);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, device.Id.ToString()),
            new(JwtRegisteredClaimNames.Name, device.FullName),
            new(AppClaims.PrisonId, device.PrisonId.ToString()),
        };
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(60),
            Issuer = _configuration[AppSettingKeysConstant.JwtSettingsIssuer],
            Audience = _configuration[AppSettingKeysConstant.JwtSettingsAudience],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);
        return jwt;
    }
}

public class PrisonerTokenQueryValidator : AbstractValidator<PrisonerTokenQuery>
{
    public PrisonerTokenQueryValidator()
    {
        RuleFor(x => x.Pid)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);
    }
}