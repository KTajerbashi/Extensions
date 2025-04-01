using Microsoft.AspNetCore.Identity;

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
