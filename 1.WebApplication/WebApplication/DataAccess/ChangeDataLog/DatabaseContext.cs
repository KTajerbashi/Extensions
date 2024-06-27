using Microsoft.EntityFrameworkCore;
using WebApplicationAPI.Model.ChangeDataLog;

namespace WebApplicationAPI.DataAccess.ChangeDataLog;

public class DatabaseContext : DbContext
{
    public DbSet<Person> People { get; set; }
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Model.SetSequenceSchema("Log");
    }

}
