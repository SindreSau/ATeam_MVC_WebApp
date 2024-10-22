using ATeam_MVC_WebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure the database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddRazorPages();

builder.Services.AddSession();

// Configure application cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";

    // Cookie settings
    options.ExpireTimeSpan = TimeSpan.FromDays(30); // The cookie will expire after X days
    options.Cookie.HttpOnly = true; // Prevents JavaScript from accessing the cookie
    options.SlidingExpiration = false; // Automatically refresh the cookie expiration time if the user is active
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensures the cookie is sent only over HTTPS

    // Max age
    options.Cookie.MaxAge = TimeSpan.FromDays(30); // The cookie will expire after X minutes

    // If the user is not authenticated, redirect to the login page
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.Redirect("/Identity/Account/Login");
        return Task.CompletedTask;
    };
});

// Configure session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(30); // The user will be logged out after X days of inactivity
    options.Cookie.HttpOnly = true; // Prevents JavaScript from accessing the cookie
    options.Cookie.IsEssential = true; // The session cookie is essential
});

var app = builder.Build();

// Configure the HTTP request pipeline.
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();