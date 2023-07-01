using JailTalk.Api;
using JailTalk.Api.Filters;
using JailTalk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"Current Directory: {Directory.GetCurrentDirectory()}");
Log.Logger = new LoggerConfiguration()
        .WriteTo.File($"{Directory.GetCurrentDirectory()}/logs/log-.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();
builder.Configuration.AddConfiguration(new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .Build());
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.RegisterService(builder.Configuration);

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.UseExceptionHandler();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
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
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
