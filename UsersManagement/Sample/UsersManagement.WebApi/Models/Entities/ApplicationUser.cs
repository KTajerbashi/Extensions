using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UsersManagement.WebApi.Models.Entities;

public class ApplicationUser : IdentityUser<long>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DisplayName { get; set; }
}


public class ApplicationUserClaim : IdentityUserClaim<long>
{
}


public class ApplicationRole : IdentityRole<long>
{
    protected ApplicationRole()
    {
        
    }
    public ApplicationRole(string name)
    {
        Name = name;
    }
}


public class ApplicationUserLogin : IdentityUserLogin<long>
{
}


public class ApplicationUserRole : IdentityUserRole<long>
{
}


public class ApplicationUserToken : IdentityUserToken<long>
{
}


public class ApplicationRoleClaim : IdentityRoleClaim<long>
{
}


public class ApplicationRefreshToken
{
    [Key]
    public string Token { get; set; }

    [Required]
    public long UserId { get; set; }

    [Required]
    public DateTime Expires { get; set; }

    [Required]
    public DateTime Created { get; set; }

    [Required]
    public bool IsRevoked { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser User { get; set; }

    public bool IsExpired => DateTime.Now >= Expires;
    public bool IsActive => !IsRevoked && !IsExpired;
}