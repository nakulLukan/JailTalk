using JailTalk.Domain.Lookup;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Contracts.Data;

public interface IAppDbContext
{
    public DbSet<LookupMaster> LookupMasters { get; set; }
}
