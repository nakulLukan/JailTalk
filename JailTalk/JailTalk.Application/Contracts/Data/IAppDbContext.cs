using JailTalk.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace JailTalk.Application.Contracts.Data;

public interface IAppDbContext
{
    public DbSet<AppUser> AspNetUsers { get; set; }
}
