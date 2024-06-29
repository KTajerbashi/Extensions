using Extensions.Events.Abstractions;
using Extensions.Events.Outbox.Extensions.Events.Outbox.Dal.EF.Configs;
using Extensions.Events.Outbox.Extensions.Events.Outbox.Dal.EF.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Extensions.Events.Outbox.Extensions.Events.Outbox.Dal.EF;

/// <summary>
/// این پایه کلاس های پایگاه داده است
/// که برای مدیریت رویداد ها روی یک شاخه لازم است ازین استفاده کنیم
/// </summary>
public abstract class BaseOutboxCommandDbContext : DbContext
{
    public virtual DbSet<OutBoxEventItem> OutBoxEventItems { get; set; }

    public BaseOutboxCommandDbContext(DbContextOptions options) : base(options) { }

    protected BaseOutboxCommandDbContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.AddInterceptors(new AddOutBoxEventItemInterceptor());
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new OutBoxEventItemConfig());
    }
}