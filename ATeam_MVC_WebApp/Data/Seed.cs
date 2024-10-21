using Microsoft.AspNetCore.Identity;
using ATeam_MVC_WebApp.Data;
using ATeam_MVC_WebApp.Models;

namespace ATeam_MVC_WebApp.Data;
public class Seed
{
    public static void SeedData(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

            context.Database.EnsureCreated();

            if (!context.FoodProducts.Any())
            {
                context.FoodProducts.AddRange(new List<FoodProduct>()
                {
                    new FoodProduct()
                    {
                        ProductName = "Banan",
                        EnergyKcal = 355,
                        Fat = 0.3M,
                        Carbohydrates = 18.3M,
                        Protein = 1.2M,
                        Fiber = 1,
                        Salt = 0

                        },
                    new FoodProduct()
                    {
                       ProductName = "Peanøtter",
                        EnergyKcal = 567,
                        Fat = 49,
                        Carbohydrates = 16,
                        Protein = 26,
                        Fiber = 9,
                        Salt = 1.8M
                    },
                    new FoodProduct()
                    {
                        ProductName = "Eple",
                        EnergyKcal = 73,
                        Fat = 0.24M,
                        Carbohydrates = 19.33M,
                        Protein = 0.36M,
                        Fiber = 4.3M,
                        Salt = 0.1M
                    },
                    new FoodProduct()
                    {
                        ProductName = "Loff",
                        EnergyKcal = 264,
                        Fat = 3.2M,
                        Carbohydrates = 18.3M,
                        Protein = 9,
                        Fiber = 2.7M,
                        Salt = 4.91M
                    }
                });
                context.SaveChanges();
            }
        }
    }

/*
    public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            //Roles
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(app.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            //Users
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            string adminUserEmail = "teddysmithdeveloper@gmail.com";

            var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
            if (adminUser == null)
            {
                var newAdminUser = new AppUser()
                {
                    UserName = "teddysmithdev",
                    Email = adminUserEmail,
                    EmailConfirmed = true,
                    Address = new Address()
                    {
                        Street = "123 Main St",
                        City = "Charlotte",
                        State = "NC"
                    }
                };
                await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
            }

            string appUserEmail = "user@etickets.com";

            var appUser = await userManager.FindByEmailAsync(appUserEmail);
            if (appUser == null)
            {
                var newAppUser = new AppUser()
                {
                    UserName = "app-user",
                    Email = appUserEmail,
                    EmailConfirmed = true,
                    Address = new Address()
                    {
                        Street = "123 Main St",
                        City = "Charlotte",
                        State = "NC"
                    }
                };
                await userManager.CreateAsync(newAppUser, "Coding@1234?");
                await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
            }
        }
    }*/
}