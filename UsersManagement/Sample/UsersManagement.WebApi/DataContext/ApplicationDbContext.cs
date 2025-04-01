using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UsersManagement.WebApi.Models.Entities;

namespace UsersManagement.WebApi.DataContext;

// Sample DbContext for Identity
public class ApplicationDbContext : IdentityDbContext
   <ApplicationUser, 
    ApplicationRole, 
    long, 
    ApplicationUserClaim, 
    ApplicationUserRole, 
    ApplicationUserLogin, 
    ApplicationRoleClaim, 
    ApplicationUserToken>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.AddSecurityConfiguration();
    }
}


public static class DataContextConfiguration
{
    public static ModelBuilder AddSecurityConfiguration(this ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>().ToTable("Users", "Security");
        builder.Entity<ApplicationUserClaim>().ToTable("UserClaims", "Security");
        builder.Entity<ApplicationUserLogin>().ToTable("UserLogins", "Security");
        builder.Entity<ApplicationUserRole>().ToTable("UserRoles", "Security");
        builder.Entity<ApplicationUserToken>().ToTable("UserTokens", "Security");
        builder.Entity<ApplicationRole>().ToTable("Roles", "Security");
        builder.Entity<ApplicationRoleClaim>().ToTable("RoleClaims", "Security");

        // Configuring IdentityUserLogin Table
        builder.Entity<ApplicationUserLogin>()
            .HasKey(l => new { l.LoginProvider, l.ProviderKey });

        return builder;
    }
}