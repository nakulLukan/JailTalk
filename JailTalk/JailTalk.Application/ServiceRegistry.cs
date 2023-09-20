using FluentValidation;
using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Impl.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JailTalk.Application;

public static class ServiceRegistry
{
    public static void RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblyContaining<Dummy>(ServiceLifetime.Transient);
        services.AddTransient<IApplicationSettingsProvider, ApplicationSettingsProvider>();
    }
}
