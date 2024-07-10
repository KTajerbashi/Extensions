using Microsoft.EntityFrameworkCore;
using WebApi.ChangeDataLog.Models.Security;

namespace WebApi.ChangeDataLog.Database;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {

    }


    public virtual DbSet<UserEntity> UserEntities { get; set; }
}
