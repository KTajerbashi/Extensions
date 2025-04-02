using Microsoft.EntityFrameworkCore;
using UsersManagement.WebApi.Models.Entities;

namespace UsersManagement.WebApi.DataContext;

public static class ApplicationDbContextConfiguration
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
        builder.Entity<ApplicationRefreshToken>().ToTable("RefreshTokens", "Security");

        // Configuring IdentityUserLogin Table
        builder.Entity<ApplicationUserLogin>()
            .HasKey(l => new { l.LoginProvider, l.ProviderKey });


        // Configure the RefreshToken entity
        builder.Entity<ApplicationRefreshToken>(entity =>
        {
            entity.HasKey(rt => rt.Token);
            entity.Property(rt => rt.Token).HasMaxLength(64);

            entity
                .HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // In ApplicationDbContext.OnModelCreating
            entity
                .HasIndex(rt => rt.UserId)
                .HasDatabaseName("IX_RefreshTokens_UserId");
        });

        return builder;
    }
}