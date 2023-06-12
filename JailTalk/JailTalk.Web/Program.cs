using JailTalk.Infrastructure.Data;
using JailTalk.Infrastructure.Data.Seeder;
using JailTalk.Web;
using JailTalk.Web.Middlewares;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConfiguration(new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
#if RELEASE
                .AddJsonFile($"appsettings.release.json", optional: true)
#endif
                .Build());
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.RegisterService(builder.Configuration);

var app = builder.Build();
try
{

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
    await dbContext.SeedRoles();
    await dbContext.SeedDefaultUsers(builder.Configuration);
}
}
catch
{

}
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<AuthenticationMiddleware>();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();
