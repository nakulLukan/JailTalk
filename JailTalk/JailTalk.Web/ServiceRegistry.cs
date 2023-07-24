using JailTalk.Application;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Contracts.Presentation;
using JailTalk.Application.Contracts.UserManagement;
using JailTalk.Domain.Identity;
using JailTalk.Infrastructure;
using JailTalk.Infrastructure.Data;
using JailTalk.Web.Contracts.Events;
using JailTalk.Web.Impl.Events;
using JailTalk.Web.Impl.Http;
using JailTalk.Web.Impl.Presentation;
using JailTalk.Web.Impl.UserManagement;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Localization;
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
            new CultureInfo("en-IN") // Set the desired culture, e.g., "en-IN" (English - India)
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
