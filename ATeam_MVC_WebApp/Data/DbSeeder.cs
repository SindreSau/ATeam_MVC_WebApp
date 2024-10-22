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
        var testId = await SeedTestUser(userManager);
        await SeedCategories(context, adminId);
        await SeedFoodProducts(context, testId);
        
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
    private static async Task<string> SeedTestUser(UserManager<IdentityUser> userManager)
    {
        const string testEmail = "test@foodapp.com";
        const string testPassword = "Test123!";

        var existingTest = await userManager.FindByEmailAsync(testEmail);
        if (existingTest != null)
        {
            return existingTest.Id;
        }

        var test = new IdentityUser
        {
            UserName = testEmail,
            Email = testEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(test, testPassword);
        if (!result.Succeeded)
        {
            throw new Exception($"Failed to create admin user: {string.Join(", ", result.Errors)}");
        }

        await userManager.AddToRoleAsync(test, "Vendor");
        return test.Id;
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
    private static async Task SeedFoodProducts(ApplicationDbContext context, string testId)
    {
        if (!context.FoodProducts.Any())
        {
            var foodProduct = new List<FoodProduct>
            {
                new FoodProduct
                {
                    ProductName = "Eple",
                    EnergyKcal = 52,
                    Fat = 0.2M, // Thin
                    Carbohydrates = 14,
                    Protein = 0.3M,
                    Fiber = 2.4M,
                    Salt = 0,
                    NokkelhullQualified = true,
                    FoodCategoryId = 1,
                    CreatedById = testId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },

                new FoodProduct
                {
                    ProductName = "Banan",
                    EnergyKcal = 89,
                    Fat = 0.3M, // Thin
                    Carbohydrates = 23,
                    Protein = 1.1M,
                    Fiber = 2.6M,
                    Salt = 0,
                    NokkelhullQualified = true,
                    FoodCategoryId = 1,
                    CreatedById = testId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },

                new FoodProduct
                {
                    ProductName = "Kyllingbryst",
                    EnergyKcal = 165,
                    Fat = 3.6M, // Thick
                    Carbohydrates = 0,
                    Protein = 31,
                    Fiber = 0,
                    Salt = 0.1M,
                    NokkelhullQualified = true,
                    FoodCategoryId = 8,
                    CreatedById = testId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },

                new FoodProduct
                {
                    ProductName = "Brød",
                    EnergyKcal = 250,
                    Fat = 4.5M, // Thick
                    Carbohydrates = 47,
                    Protein = 8.5M,
                    Fiber = 3.5M,
                    Salt = 1.2M,
                    NokkelhullQualified = false,
                    FoodCategoryId = 3,
                    CreatedById = testId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },

                new FoodProduct
                {
                    ProductName = "Yoghurt naturell",
                    EnergyKcal = 59,
                    Fat = 3.3M, // Thick
                    Carbohydrates = 4.7M,
                    Protein = 3.5M,
                    Fiber = 0,
                    Salt = 0.1M,
                    NokkelhullQualified = true,
                    FoodCategoryId = 4,
                    CreatedById = testId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },

                new FoodProduct
                {
                    ProductName = "Brokkoli",
                    EnergyKcal = 34,
                    Fat = 0.4M, // Thin
                    Carbohydrates = 7,
                    Protein = 2.8M,
                    Fiber = 2.6M,
                    Salt = 0,
                    NokkelhullQualified = true,
                    FoodCategoryId = 1,
                    CreatedById = testId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },

                new FoodProduct
                {
                    ProductName = "Laks",
                    EnergyKcal = 208,
                    Fat = 13, // Thick
                    Carbohydrates = 0,
                    Protein = 20,
                    Fiber = 0,
                    Salt = 0.1M,
                    NokkelhullQualified = true,
                    FoodCategoryId = 7,
                    CreatedById = testId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },

                new FoodProduct
                {
                    ProductName = "Havregryn",
                    EnergyKcal = 389,
                    Fat = 7, // Thick
                    Carbohydrates = 66,
                    Protein = 17,
                    Fiber = 10.6M,
                    Salt = 0.01M,
                    NokkelhullQualified = true,
                    FoodCategoryId = 3,
                    CreatedById = testId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },

                new FoodProduct
                {
                    ProductName = "Skummet melk",
                    EnergyKcal = 34,
                    Fat = 0.1M, // Thin
                    Carbohydrates = 5,
                    Protein = 3.4M,
                    Fiber = 0,
                    Salt = 0.1M,
                    NokkelhullQualified = true,
                    FoodCategoryId = 4,
                    CreatedById = testId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },

                new FoodProduct
                {
                    ProductName = "Egg",
                    EnergyKcal = 155,
                    Fat = 11, // Thick
                    Carbohydrates = 1.1M,
                    Protein = 13,
                    Fiber = 0,
                    Salt = 0.4M,
                    NokkelhullQualified = false,
                    FoodCategoryId = 2,
                    CreatedById = testId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            await context.FoodProducts.AddRangeAsync(foodProduct);
            await context.SaveChangesAsync();
        }
    }
}