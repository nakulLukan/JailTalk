using JailTalk.Api.Impl.UserManagement;
using JailTalk.Application;
using JailTalk.Application.Contracts.UserManagement;
using JailTalk.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JailTalk.Api;

public static class ServiceRegistry
{
    public static void RegisterService(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterApplicationServices(configuration);
        services.RegisterInfrastructureServices(configuration);
        RegisterWebServices(services, configuration);
    }

    private static void RegisterWebServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddAuthentication(auth =>
        {
            auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(jwt =>
        {
            jwt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidIssuer = configuration["JwtSettings:Issuer"],
                ValidAudience = configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            };
        });
        services.AddAuthorization();

        services.AddMediatR((c) =>
        {
            c.RegisterServicesFromAssembly(typeof(Application.ServiceRegistry).Assembly);
        });
    }
}
