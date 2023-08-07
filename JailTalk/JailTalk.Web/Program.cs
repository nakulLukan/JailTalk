using JailTalk.Domain.Identity;
using JailTalk.Infrastructure.Data;
using JailTalk.Infrastructure.Data.Seeder;
using JailTalk.Web;
using JailTalk.Web.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddConfiguration(new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appconfig.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .Build());

Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();
Log.Logger.Information($"Current Directory: {Directory.GetCurrentDirectory()}");
builder.Services.RegisterService(builder.Configuration);

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
    await dbContext.SeedRoles(roleManager);
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    await dbContext.SeedDefaultUsers(builder.Configuration, userManager);
}
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRequestLocalization();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<AuthenticationMiddleware>();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();
