using JailTalk.Application.Contracts.AI;
using JailTalk.Application.Contracts.Audio;
using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Email;
using JailTalk.Application.Contracts.Graphics;
using JailTalk.Application.Contracts.Storage;
using JailTalk.Infrastructure.Data;
using JailTalk.Infrastructure.Impl.AI;
using JailTalk.Infrastructure.Impl.Audio;
using JailTalk.Infrastructure.Impl.Email;
using JailTalk.Infrastructure.Impl.Graphics;
using JailTalk.Infrastructure.Impl.Storage;
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
            .UseSnakeCaseNamingConvention(), ServiceLifetime.Transient);
        services.AddTransient<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        services.AddTransient<IAppImageEditor, AppImageEditor>();
        services.AddTransient<IAppFaceRecognition, AwsFaceRecognition>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IFileStorage, AwsStorage>();
        services.AddTransient<IAudioService, AudioService>();
    }
}
