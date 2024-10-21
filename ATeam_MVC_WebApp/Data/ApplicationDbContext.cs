using ATeam_MVC_WebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ATeam_MVC_WebApp.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Add new DbSet for each new model
    public DbSet<FoodCategory> FoodCategories { get; set; }
    public DbSet<FoodProduct> FoodProducts { get; set; }

    // TODO: Override OnModelCreating to seed data
    // protected override void OnModelCreating(ModelBuilder builder)
    // {
    //     base.OnModelCreating(builder);
    //     builder.Entity<IdentityRole>().HasData(
    //         new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
    //         new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
    //     );
    // }
}