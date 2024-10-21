using ATeam_MVC_WebApp.Data;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

// === SERILOG CONFIGURATION === //
// Log.Logger = new LoggerConfiguration()
//     .MinimumLevel.Debug()
//     .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
//     .Enrich.FromLogContext()
//     .WriteTo.Console()
//     .WriteTo.File("logs/log-.log", rollingInterval: RollingInterval.Day)
//     .WriteTo.Seq("http://localhost:8080")
//     .CreateBootstrapLogger();

try
{
    Log.Information("Starting web application");
    Log.Information("Creating builder...");

    // === BUILDER CONFIGURATION === //
    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    // builder.Host.UseSerilog((context, services, configuration) => configuration
    //     .ReadFrom.Configuration(context.Configuration)
    //     .ReadFrom.Services(services)
    //     .Enrich.FromLogContext()
    //     .WriteTo.Console()
    //     .WriteTo.File("logs/log-.log", rollingInterval: RollingInterval.Day)
    //     .WriteTo.Seq("http://localhost:8080"));

    // Add controllers with support for views
    builder.Services.AddControllersWithViews();

    // Configure the database
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddRazorPages(); // Add Razor Pages as these are used for Identity

    // === APP CONFIGURATION === //
    Log.Information("Configuring application...");
    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    Log.Information("Application started");
    Log.Information("Application is running on https://localhost:7177/");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}