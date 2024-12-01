using ATeam_MVC_WebApp.Configuration;
using Microsoft.AspNetCore.Identity;
using ATeam_MVC_WebApp.Data;
using ATeam_MVC_WebApp.Middleware;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Serilog;
using Serilog.Events;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    // Console output - keep multi-line for development readability
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}Properties: {Properties:j}{NewLine}{Exception}")
    // File output - single line, concise format
    .WriteTo.File(
        outputTemplate:
        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj} | Method={RequestMethod} Path={RequestPath} Status={StatusCode} Elapsed={ElapsedMilliseconds}ms User={UserId} Client={ClientIP} Query={QueryString}{NewLine}",
        path: "logs/log-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30)
    .CreateLogger();

Log.Information("Starting web application");

var builder = WebApplication.CreateBuilder(args);

// Set the urls for the web application
builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001");

// Remove default logging providers and add Serilog
builder.Host.UseSerilog();

// Add services to the container
builder.Services
    .AddControllersWithViews()
    .Services.AddMvc()
    .AddRazorPagesOptions(options => { });

// Configure services using extension methods from the Configuration folder
builder.Services
    .AddDatabaseServices(builder.Configuration)
    .AddIdentityServices()
    .AddSessionServices();

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var context = services.GetRequiredService<ApplicationDbContext>();

        Log.Information("Seeding database...");

        await DbSeeder.SeedData(services, userManager, roleManager);

        if (app.Environment.IsDevelopment())
        {
            Log.Information("Development environment detected, seeding test data...");
            await DbSeeder.SeedTestVendorsWithProducts(userManager, context);
        }

        Log.Information("Database seeding completed successfully");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while seeding the database");
        throw; // Re-throw after logging
    }
}

// Configure the middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    Log.Information("Running in production mode");
}
else
{
    Log.Information("Running in development mode");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLogging();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

Log.Information("Web application started successfully");
Log.Information("Web application listening on http://localhost:5000 and https://localhost:5001");
await app.RunAsync();
