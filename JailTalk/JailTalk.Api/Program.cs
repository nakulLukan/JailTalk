using JailTalk.Api;
using JailTalk.Infrastructure.Impl.Middlewares;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConfiguration(new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appconfig.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .Build());

Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();
Log.Logger.Information($"Current Directory: {Directory.GetCurrentDirectory()}");

builder.Services.RegisterService(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<AppApiGlobalExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Jail Talk Device API V1");

    // Configure the Swagger UI to require authorization
    c.DocumentTitle = "Jail Talk Device";
    c.DocExpansion(DocExpansion.None);
    c.DefaultModelExpandDepth(0);
    c.EnableDeepLinking();
    c.EnableFilter();
    c.DisplayRequestDuration();
    c.EnableValidator();
});
app.Run();
