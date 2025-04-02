using Microsoft.AspNetCore.Identity;
using UsersManagement.WebApi.Interfaces;
using UsersManagement.WebApi.Models.Entities;

namespace UsersManagement.WebApi.Services;



public class IdentityRespository : IIdentityRespository
{
    private SignInManager<ApplicationUser> _signInManager;
    public SignInManager<ApplicationUser> SignInManager => _signInManager;

    private UserManager<ApplicationUser> _userManager;
    public UserManager<ApplicationUser> UserManager => _userManager;

    private RoleManager<ApplicationRole> _roleManager; 
    public RoleManager<ApplicationRole> RoleManager => _roleManager;

    public IConfiguration _configuration;
    public IConfiguration Configuration => _configuration;

    public IdentityRespository(
        SignInManager<ApplicationUser> signInManager, 
        UserManager<ApplicationUser> userManager, 
        RoleManager<ApplicationRole> roleManager, 
        IConfiguration configuration)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }


}