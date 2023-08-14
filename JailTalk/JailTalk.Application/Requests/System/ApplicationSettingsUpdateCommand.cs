using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Dto.System;
using JailTalk.Application.Services;
using JailTalk.Shared;
using JailTalk.Shared.Constants;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace JailTalk.Application.Requests.System;

public class ApplicationSettingsUpdateCommand : IRequest<ResponseDto<ApplicationSettingsListDto>>
{
    [MaxLength(100)]
    [RegularExpression(RegularExpressionPatternConstant.ApplicationSettingsKey)]
    public string Key { get; set; }

    public string Value { get; set; }
}

public class ApplicationSettingsUpdateCommandHandler : IRequestHandler<ApplicationSettingsUpdateCommand, ResponseDto<ApplicationSettingsListDto>>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;

    public ApplicationSettingsUpdateCommandHandler(IAppDbContext dbContext, IAppRequestContext requestContext = null)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
    }

    public async Task<ResponseDto<ApplicationSettingsListDto>> Handle(ApplicationSettingsUpdateCommand request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse(request.Key, true, out ApplicationSettings key))
        {
            throw new AppException("Application settings could not be found");
        }
        var applicationSetting = await _dbContext.ApplicationSettings.AsTracking()
            .FirstOrDefaultAsync(x => x.Id == key, cancellationToken);
        if (applicationSetting == null)
        {
            throw new AppException("Unknown application settings.");
        }

        if (!Regex.IsMatch(request.Value, applicationSetting.RegexValidation))
        {
            throw new AppException("Value not in expected format. Please check the application setting validation");
        }

        if (applicationSetting.IsReadonly)
        {
            throw new AppException("Cannot update a read-only access application settings value.");
        }
        applicationSetting.Value = request.Value;
        applicationSetting.LastUpdatedOn = AppDateTime.UtcNow;
        applicationSetting.LastUpdateBy = await _requestContext.GetUserName();
        await _dbContext.SaveAsync(cancellationToken);
        return new(ApplicationSettingsService.GetApplicationSettingsDetails(applicationSetting));
    }
}

