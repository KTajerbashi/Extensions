using Microsoft.EntityFrameworkCore;
using WebApplicationAPI.Model.ChangeDataLog;

namespace WebApplicationAPI.DataAccess.ChangeDataLog;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Person> People { get; set; }

}
