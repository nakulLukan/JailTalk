using JailTalk.Application.Contracts.Data;
using JailTalk.Shared;
using JailTalk.Shared.Utilities;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Services;

public class PrisonerHelper
{
    #region Mapper for prisoner id to PID
    private static Semaphore _prionserIdToCodeMapperMutex = new Semaphore(1, 1);
    private static IDictionary<Guid, string> _prionserIdToCodeMapper = null;


    public static async Task<string> GetPrisonerCodeFromId(Guid prisonerId, IAppDbContext dbContext)
    {
        await CheckAndLoadPrisonerIdToPidLookup(dbContext);
        if (!_prionserIdToCodeMapper.ContainsKey(prisonerId))
        {
            throw new AppException(CommonExceptionMessages.PrisonerNotFound);
        }

        return _prionserIdToCodeMapper[prisonerId];
    }

    private static async Task CheckAndLoadPrisonerIdToPidLookup(IAppDbContext dbContext)
    {
        _prionserIdToCodeMapperMutex.WaitOne();
        if (_prionserIdToCodeMapper == null)
        {
            _prionserIdToCodeMapper = await dbContext.Prisoners
            .Select(x => new
            {
                x.Id,
                x.Pid
            })
            .ToDictionaryAsync(x => x.Id, x => x.Pid);
        }
        _prionserIdToCodeMapperMutex.Release();
    }
    #endregion

    public static bool IsUnlimitedCallPriviledgeEnabled(DateTimeOffset? allowedTill)
        => allowedTill.HasValue ? allowedTill.Value > AppDateTime.UtcNow : false;


    public static bool IsPrisonerBlocked(bool isPermanentlyBlocked, DateTimeOffset? punishmentEndsOn)
    {
        return isPermanentlyBlocked || punishmentEndsOn.HasValue && punishmentEndsOn.Value > AppDateTime.UtcNow;
    }

    /// <summary>
    /// Converts given <paramref name="isActive"/> and <paramref name="isBlocked"/> to text representation
    /// </summary>
    /// <param name="isActive"></param>
    /// <param name="isBlocked"></param>
    /// <returns></returns>
    public static string ConvertContactStateAsText(bool isActive, bool isBlocked)
    {
        return isActive switch
        {
            true when !isBlocked => "Enabled",
            true when isBlocked => "Enabled, Blocked",
            false when !isBlocked => "Disabled",
            false when isBlocked => "Disabled, Blocked",
            _ => string.Empty
        };
    }

    #region Get storage base path for all prisoner related attachments
    public static async Task<string> GetPrisonerAttachmentBasePath(Guid prisonerId, IAppDbContext dbContext)
    {
        var pid = await GetPrisonerCodeFromId(prisonerId, dbContext);
        return GetPrisonerAttachmentBasePath(pid);
    }

    /// <summary>
    /// Get prisoners base attachment path
    /// </summary>
    /// <param name="pid"></param>
    /// <returns></returns>
    public static string GetPrisonerAttachmentBasePath(string pid)
    {
        return $"prisoners/{pid}";
    }
    #endregion
}
