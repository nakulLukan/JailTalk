﻿using JailTalk.Application.Contracts.Data;
using JailTalk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JailTalk.Infrastructure;

public static class ServiceRegistry
{
    public static void RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        services.AddDbContext<AppDbContext>(options =>
            options
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention()
            .LogTo(Serilog.Log.Logger.Information));
        services.AddTransient<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
    }
}