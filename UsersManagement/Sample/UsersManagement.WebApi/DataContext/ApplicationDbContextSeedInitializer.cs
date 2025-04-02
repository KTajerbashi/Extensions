using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UsersManagement.WebApi.Models.Entities;

namespace UsersManagement.WebApi.DataContext;

public class ApplicationDbContextSeedInitializer
{
    private readonly ILogger<ApplicationDbContextSeedInitializer> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public ApplicationDbContextSeedInitializer(
        ILogger<ApplicationDbContextSeedInitializer> logger,
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager
        )
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task ExecuteAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();

        }
        catch (Exception)
        {

            throw;
        }
    }


    //private async Task SeedRolesAsync()
    //{
    //    var administratorRole = new RoleEntity(Roles.Administrator, "Admin");

    //    if (await _roleManager.Roles.AllAsync(r => r.Name != administratorRole.Name))
    //    {
    //        await _roleManager.CreateAsync(administratorRole);
    //    }
    //}

    //public async Task SeedAsync()
    //{
    //    try
    //    {
    //        await SeedRolesAsync();
    //        await SeedUsersAsync();
    //        await SeedDefaultDataAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "An error occurred while seeding the database.");
    //        throw;
    //    }
    //}
    //private async Task SeedUsersAsync()
    //{
    //    await createUser("tajerbashi", "@Tajerbashi123", "9000001110");
    //    await createUser("mcbehruz", "@McBehruz123", "4000110004");
    //    await createUser("mirzaie", "@Mirzaie123", "2100100106");
    //    await createUser("saied_sayed", "@Saied123", "4010110104");
    //    await createUser("mehdi_mo", "@Mehdi123", "9632011112");
    //}
    //private async Task createUser(string username, string password, string nationalCode)
    //{
    //    var administrator = new ApplicationUser(username,username,"",$"{username}@mail.com",username,password);
    //    administrator.UserName = username;
    //    administrator.Email = $"{username}@manmail.ir";

    //    // Check if the username or email already exists
    //    if (!await _userManager.Users.AnyAsync(u => u.UserName == administrator.UserName && u.Email == administrator.Email))
    //    {
    //        var createResult = await _userManager.CreateAsync(administrator, password);
    //        if (createResult.Succeeded)
    //        {
    //            // Manually create a UserRoleEntity
    //            var userRoleEntity = new ApplicationUserRole();
    //            var role = _context.Set<ApplicationRole>().Single(item => item.Name == Roles.Administrator);
    //            userRoleEntity.Create(administrator.Id, role.Id, true);
    //            // Add the user role entity to your context
    //            _context.UserRoles.Add(userRoleEntity);
    //            await _context.SaveChangesAsync();  // Save changes to commit the role assignment

    //            _logger.LogInformation($"User {administrator.UserName} assigned to Administrator role.");
    //        }
    //        else
    //        {
    //            _logger.LogError($"Failed to create {username} user. Errors: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
    //        }
    //    }
    //    else
    //    {
    //        _logger.LogWarning($"User with username '{username}' or email '{administrator.Email}' already exists.");
    //    }
    //}


    //private async Task SeedDefaultDataAsync()
    //{
    //    if (!await _context.Users.AnyAsync())
    //    {


    //        await _context.SaveChangesAsync();
    //    }
    //}

}