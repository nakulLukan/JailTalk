using JailTalk.Api.Filters;
using JailTalk.Api.Impl.Http;
using JailTalk.Application;
using JailTalk.Application.Contracts.Http;
using JailTalk.Application.Contracts.UserManagement;
using JailTalk.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
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
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSerilog(dispose: true);
        });
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHttpContextAccessor();
        services.AddMemoryCache();
        services.AddScoped<IAuthenticationService>(prv => null);
        services.AddScoped<IDeviceRequestContext, DeviceRequestContext>();
        services.AddScoped<IAppRequestContext>(prv => null);
        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc("v1", new OpenApiInfo { Title = "Jail Talk Device Api", Version = "v1" });
            config.OperationFilter<SwaggerHeaderFilter>();
            config.OperationFilter<SwaggerBearerTokenFilter>();
            config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            config.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });

            config.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
            config.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                $"{typeof(Application.ServiceRegistry).Assembly.GetName().Name}.xml"));
            config.UseInlineDefinitionsForEnums();
        });
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

        services.AddProblemDetails();
    }
}
