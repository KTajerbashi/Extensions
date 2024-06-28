using Layer.Domain.Library.Model.ChangeDataLog;
using Microsoft.EntityFrameworkCore;

namespace Layer.Infrastructure.Library.DataAccess.ChangeDataLog;

public class BaseDatabaseContext : DbContext
{
    public BaseDatabaseContext(DbContextOptions options) : base(options)
    {
    }

}
public class DatabaseContext : BaseDatabaseContext
{
    public DbSet<Person> People { get; set; }
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

}