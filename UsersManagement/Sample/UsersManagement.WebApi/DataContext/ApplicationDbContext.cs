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

    public virtual DbSet<ApplicationRefreshToken> ApplicationRefreshTokens => Set<ApplicationRefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.AddSecurityConfiguration();
    }
}
