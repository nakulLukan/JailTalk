﻿using JailTalk.Application;
using JailTalk.Application.Contracts.UserManagement;
using JailTalk.Domain.Identity;
using JailTalk.Infrastructure;
using JailTalk.Infrastructure.Data;
using JailTalk.Web.Contracts.Events;
using JailTalk.Web.Impl.Events;
using JailTalk.Web.Impl.UserManagement;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace JailTalk.Web;

public static class ServiceRegistry
{
    public static void RegisterService(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterApplicationServices(configuration);
        services.RegisterInfrastructureServices(configuration);
        RegisterIdentity(services);
        RegisterWebServices(services);
    }

    private static void RegisterWebServices(IServiceCollection services)
    {
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddMediatR((c)=>
        {
            c.RegisterServicesFromAssembly(typeof(Application.ServiceRegistry).Assembly);
        });
        services.AddTransient<IAppMediator, AppMediator>();
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
