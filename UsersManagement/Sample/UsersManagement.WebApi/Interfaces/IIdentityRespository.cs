using Microsoft.AspNetCore.Identity;
using UsersManagement.WebApi.Models.Entities;

namespace UsersManagement.WebApi.Interfaces;

public interface IIdentityRespository
{
    SignInManager<ApplicationUser> SignInManager { get; }
    UserManager<ApplicationUser> UserManager { get; }
    RoleManager<ApplicationRole> RoleManager { get; }
    IConfiguration Configuration { get; }
}
