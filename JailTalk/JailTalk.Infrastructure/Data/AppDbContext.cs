using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Domain.Identity;
using JailTalk.Domain.Lookup;
using JailTalk.Domain.Prison;
using JailTalk.Domain.System;
using JailTalk.Infrastructure.Data.Seeder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace JailTalk.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>, IAppDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly(), t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)
            )
        );

        modelBuilder.SeedData();
    }

    public Task<int> SaveAsync(CancellationToken cancellationToken)
    {
        return SaveChangesAsync(cancellationToken);
    }

    public void ClearChanges()
    {
        this.ChangeTracker.Clear();
    }

    public DbSet<LookupMaster> LookupMasters { get; set; }
    public DbSet<LookupDetail> LookupDetails { get; set; }
    public DbSet<AddressBook> AddressBook { get; set; }
    public DbSet<CallHistory> CallHistory { get; set; }
    public DbSet<Jail> Jails { get; set; }
    public DbSet<PhoneBalance> PhoneBalances { get; set; }
    public DbSet<PhoneBalanceHistory> PhoneBalanceHistory { get; set; }
    public DbSet<PhoneDirectory> PhoneDirectory { get; set; }
    public DbSet<Prisoner> Prisoners { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<ApplicationSetting> ApplicationSettings { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<PrisonerFaceEncodingMapping> PrisonerFaceEncodingMappings { get; set; }
    public DbSet<AppFaceEncoding> AppFaceEncodings { get; set; }
}
