using ATeam_MVC_WebApp.Data;
using ATeam_MVC_WebApp.Interfaces;
using ATeam_MVC_WebApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ATeam_MVC_WebApp.Configuration;

public static class DatabaseConfig
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IFoodProductRepository, FoodProductRepository>();
        services.AddScoped<IFoodCategoryRepository, FoodCategoryRepository>();

        return services;
    }
}