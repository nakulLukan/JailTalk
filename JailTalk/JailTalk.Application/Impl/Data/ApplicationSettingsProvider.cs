using JailTalk.Application.Contracts.Data;
using JailTalk.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace JailTalk.Application.Impl.Data;

public class ApplicationSettingsProvider : IApplicationSettingsProvider
{
    readonly IServiceProvider _serviceProvider;
    readonly IMemoryCache _memoryCache;
    static readonly MemoryCacheEntryOptions _cacheOptions = new MemoryCacheEntryOptions
    {
        SlidingExpiration = TimeSpan.FromMinutes(5)
    };

    public ApplicationSettingsProvider(
        IServiceProvider serviceProvider,
        IMemoryCache memoryCache)
    {
        _serviceProvider = serviceProvider;
        _memoryCache = memoryCache;
    }

    public async Task<int> GetMaxAllowedCallDuration(Gender gender)
    {
        return gender switch
        {
            Gender.Male => int.Parse(await GetApplicationSettingValue(ApplicationSettings.MaxAllowedCallDurationMale)),
            Gender.Female => int.Parse(await GetApplicationSettingValue(ApplicationSettings.MaxAllowedCallDurationFemale)),
            Gender.Others => int.Parse(await GetApplicationSettingValue(ApplicationSettings.MaxAllowedCallDurationOthers)),
            _ => throw new ArgumentException($"{nameof(gender)}: {gender}"),
        };
    }

    private async Task<string> GetApplicationSettingValue(ApplicationSettings applicationSetting)
    {
        if (_memoryCache.TryGetValue(applicationSetting, out string value))
        {
            return value;
        }
        else
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
            var appsetting = await dbContext.ApplicationSettings.FirstAsync(x => x.Id == applicationSetting);
            _memoryCache.Set(applicationSetting, appsetting.Value, _cacheOptions);
            return appsetting.Value;
        }
    }
}
