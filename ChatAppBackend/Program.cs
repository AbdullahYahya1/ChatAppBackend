using Business.EmailSender;
using Business.Hubs;
using ChatAppBackend.Configuration;
using Hangfire;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSwagger()
    .AddDatabase(builder.Configuration)
    .AddRepositoriesInjections()
    .AddServicesInjections()
    .AddAuthenticationAndAuthorization(builder.Configuration)
    .AddJson()
    .AddHealthChecks();
builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
//app.Lifetime.ApplicationStarted.Register(() =>
//{
//    using (var scope = app.Services.CreateScope())
//    {
//        var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
//        var backgroundJobService = scope.ServiceProvider.GetRequiredService<BackgroundJobService>();
//        recurringJobManager.AddOrUpdate("healthCheck-job", () => backgroundJobService.HealthCheck(), Cron.HourInterval(1));
//    }
//});

app.MapHub<UserHub>("/userHub");
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowSpecificOrigin");
app.MapControllers();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=1204800"); // Cache for 14 days
    }
}); 
app.Run();
