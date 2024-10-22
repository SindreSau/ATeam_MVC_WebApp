using ATeam_MVC_WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ATeam_MVC_WebApp.Data;

public static class DbSeeder
{
    public static async Task SeedData(
        IServiceProvider serviceProvider,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Ensure database is created and migrations are applied
        await context.Database.EnsureCreatedAsync();
        await context.Database.MigrateAsync();

        await SeedRoles(roleManager);
        var adminId = await SeedAdmin(userManager);
        await SeedCategories(context, adminId);
    }

    private static async Task<string> SeedAdmin(UserManager<IdentityUser> userManager)
    {
        const string adminEmail = "admin@foodapp.com";
        const string adminPassword = "Admin123!";

        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
        if (existingAdmin != null)
        {
            return existingAdmin.Id;
        }

        var admin = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(admin, adminPassword);
        if (!result.Succeeded)
        {
            throw new Exception($"Failed to create admin user: {string.Join(", ", result.Errors)}");
        }

        await userManager.AddToRoleAsync(admin, "Admin");
        return admin.Id;
    }

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        string[] roleNames = { "Admin", "Vendor" };

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    private static async Task SeedCategories(ApplicationDbContext context, string adminId)
    {
        if (!context.FoodCategories.Any())
        {
            var categories = new[]
            {
                "Grønnsaker, frukt, bær og nøtter",
                "Mel, gryn og ris",
                "Grøt, brød og pasta",
                "Melk, syrnede melkeprodukter og vegetabilske alternativer",
                "Ost og vegetabilske alternativer",
                "Matfett og oljer",
                "Fisk, skalldyr og produkter av disse",
                "Kjøtt, pålegg, pølser etc.",
                "Vegetabilske produkter",
                "Ferdigretter",
                "Dressinger og sauser"
            };

            var now = DateTime.UtcNow;
            var categoryEntities = categories.Select(name => new FoodCategory
            {
                CategoryName = name,
                CreatedAt = now,
                UpdatedAt = now
            });

            await context.FoodCategories.AddRangeAsync(categoryEntities);
            await context.SaveChangesAsync();
        }
    }
}