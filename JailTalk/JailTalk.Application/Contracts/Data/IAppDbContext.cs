﻿using JailTalk.Domain.Identity;
using JailTalk.Domain.Lookup;
using JailTalk.Domain.Prison;
using JailTalk.Domain.System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
    public DbSet<PrisonerFunction> PrisonerFunctions { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<ApplicationSetting> ApplicationSettings { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<PrisonerFaceEncodingMapping> PrisonerFaceEncodingMappings { get; set; }
    public DbSet<AppUser> Users { get; set; }
    public DbSet<AppRole> Roles { get; set; }
    public DbSet<IdentityUserRole<string>> UserRoles { get; set; }
    public DbSet<JailAccountRechargeRequest> JailAccountRechargeRequests { get; set; }
    public DbSet<JailAccountBalance> JailAccountBalance { get; set; }

    public void ClearChanges();

    public void Set<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> property, TProperty value);

    Task<int> SaveAsync(CancellationToken cancellationToken);
}
