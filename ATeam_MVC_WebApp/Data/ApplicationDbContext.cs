using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ATeam_MVC_WebApp.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // protected override void OnModelCreating(ModelBuilder builder)
    // {
    //     base.OnModelCreating(builder);
    //     builder.Entity<IdentityRole>().HasData(
    //         new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
    //         new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
    //     );
    // }
}