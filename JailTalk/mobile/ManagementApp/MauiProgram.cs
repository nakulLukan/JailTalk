using Management.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using Refit;
using System.Reflection;

namespace ManagementApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            using var defautlSetting = Assembly.GetExecutingAssembly().GetManifestResourceStream("ManagementApp.appsettings.json")!;
            var configuration = new ConfigurationBuilder()
                    .AddJsonStream(defautlSetting)
                    .Build();
            builder.Configuration.AddConfiguration(configuration);

            builder.Services.AddMudServices();
            builder.Services.AddHttpClient();
            builder.Services
                .AddRefitClient<IPrisonAccountManagementApiService>()
                .ConfigurePrimaryHttpMessageHandler(c =>
                {
                    var handler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
                    };

                    return handler;
                })
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration["RemoteServices:JailConnectApi"]));

            return builder.Build();
        }
    }
}
