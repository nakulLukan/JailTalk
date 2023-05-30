﻿using JailTalk.Application;
using JailTalk.Domain.Identity;
using JailTalk.Infrastructure;
using JailTalk.Infrastructure.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;

namespace JailTalk.Web;

public static class ServiceRegistry
{
    public static void RegisterService(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterApplicationServices(configuration);
        services.RegisterInfrastructureServices(configuration);
        RegisterIdentity(services);
    }

    private static void RegisterIdentity(IServiceCollection services)
    {
        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedEmail = false;
        })
            .AddEntityFrameworkStores<AppDbContext>();
        services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
    }
}