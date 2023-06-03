using JailTalk.Domain.Identity;
using JailTalk.Domain.Lookup;
using JailTalk.Domain.Prison;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Contracts.Data;

public interface IAppDbContext
{
    public DbSet<LookupMaster> LookupMasters { get; set; }
    public DbSet<LookupDetail> LookupDetails { get; set; }
    public DbSet<AddressBook> AddressBook { get; set; }
    public DbSet<CallHistory> CallHistory { get; set; }
    public DbSet<Jail> Jails { get; set; }
    public DbSet<PhoneBalance> PhoneBalances { get; set; }
    public DbSet<PhoneBalanceHistory> PhoneBalanceHistory { get; set; }
    public DbSet<PhoneDirectory> PhoneDirectory { get; set; }
    public DbSet<Prisoner> Prisoners { get; set; }

    Task<int> SaveAsync(CancellationToken cancellationToken);
}
