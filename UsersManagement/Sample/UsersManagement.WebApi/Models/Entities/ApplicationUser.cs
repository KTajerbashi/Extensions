using Microsoft.AspNetCore.Identity;

namespace UsersManagement.WebApi.Models.Entities;

public class ApplicationUser : IdentityUser<long>
{
}


public class ApplicationUserClaim : IdentityUserClaim<long>
{
}


public class ApplicationRole : IdentityRole<long>
{
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
