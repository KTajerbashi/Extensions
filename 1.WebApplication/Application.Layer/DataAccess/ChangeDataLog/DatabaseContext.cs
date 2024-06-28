using Application.Layer.Model.ChangeDataLog;
using Microsoft.EntityFrameworkCore;

namespace Application.Layer.DataAccess.ChangeDataLog;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Person> People { get; set; }

}
