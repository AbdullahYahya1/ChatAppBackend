using Business.EmailSender;
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
builder.Services.AddHttpContextAccessor();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
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
app.UseAuthorization();
app.UseAuthentication();
app.UseCors("AllowSpecificOrigin");
app.MapControllers();
app.UseStaticFiles();
app.Run();
