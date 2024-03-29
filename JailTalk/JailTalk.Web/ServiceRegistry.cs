﻿using Fluxor;
using JailTalk.Application;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Contracts.Presentation;
using JailTalk.Application.Contracts.UserManagement;
using JailTalk.Domain.Identity;
using JailTalk.Infrastructure;
using JailTalk.Infrastructure.Data;
using JailTalk.Web.Contracts.Events;
using JailTalk.Web.Contracts.Interop;
using JailTalk.Web.Impl.Events;
using JailTalk.Web.Impl.Http;
using JailTalk.Web.Impl.Identity;
using JailTalk.Web.Impl.Interop;
using JailTalk.Web.Impl.Presentation;
using JailTalk.Web.Impl.UserManagement;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Localization;
using MudBlazor.Services;
using Serilog;
using System.Globalization;

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
        services.AddScoped<IAppJSInterop, AppJSInterop>();
        services.AddScoped<IAppAuthenticator, AppAuthenticator>();
        services.AddFluxor(options => options.ScanAssemblies(typeof(Program).Assembly));
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSerilog(dispose: true);
        });
        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddMudServices();
        services.AddMemoryCache();
        services.AddTransient<IToastService, ToastService>();
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddMediatR((c) =>
        {
            c.RegisterServicesFromAssembly(typeof(Application.ServiceRegistry).Assembly);
        });
        services.AddTransient<IAppMediator, AppMediator>();
        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en-IN")
            };

            options.DefaultRequestCulture = new RequestCulture(supportedCultures[0]);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });
    }

    private static void RegisterIdentity(IServiceCollection services)
    {
        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.SignIn.RequireConfirmedEmail = false;
        })
            .AddEntityFrameworkStores<AppDbContext>();
        services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
        services.AddScoped<IAppRequestContext, AppRequestContext>();
        services.AddScoped<IDeviceRequestContext>(prv => null);
    }
}
