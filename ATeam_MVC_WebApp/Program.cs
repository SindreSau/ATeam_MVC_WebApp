using ATeam_MVC_WebApp.Data;
using Microsoft.EntityFrameworkCore;

// === BUILDER CONFIGURATION === //
var builder = WebApplication.CreateBuilder(args);

// Add controllers with support for views
builder.Services.AddControllersWithViews();

// Configure the database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRazorPages(); // Add Razor Pages as these are used for Identity


// === APP CONFIGURATION === //
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

app.Run();