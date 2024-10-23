// Program.cs

using ATeam_MVC_WebApp.Configuration;
using Microsoft.AspNetCore.Identity;
using ATeam_MVC_WebApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services
    .AddControllersWithViews()
    .Services.AddMvc() // Add MVC services (also adds Razor Pages)
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

        await DbSeeder.SeedData(services, userManager, roleManager);

        if (app.Environment.IsDevelopment())
        {
            await DbSeeder.SeedTestVendorWithTestProducts(userManager, context);
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Vendor}/{action=Index}/{id?}");

app.MapRazorPages();

await app.RunAsync();