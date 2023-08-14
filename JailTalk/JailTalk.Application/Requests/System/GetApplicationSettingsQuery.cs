using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Dto.System;
using JailTalk.Application.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Requests.System;

public class GetApplicationSettingsQuery : IRequest<List<ApplicationSettingsListDto>>
{
}

public class GetApplicationSettingsQueryHandler : IRequestHandler<GetApplicationSettingsQuery, List<ApplicationSettingsListDto>>
{
    readonly IAppDbContext _dbContext;

    public GetApplicationSettingsQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ApplicationSettingsListDto>> Handle(GetApplicationSettingsQuery request, CancellationToken cancellationToken)
    {
        var applicationSettings = await _dbContext.ApplicationSettings.ToListAsync(cancellationToken);
        return applicationSettings
            .Select(x => ApplicationSettingsService.GetApplicationSettingsDetails(x))
            .OrderBy(x => x.ApplicationSettingId).ToList();
    }
}

