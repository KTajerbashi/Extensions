using Microsoft.EntityFrameworkCore;
using WebApi.ChangeDataLog.Database;
using WebApi.ChangeDataLog.Models.Bases;
using WebApi.ChangeDataLog.Repositories.Bases;

namespace WebApi.ChangeDataLog.Services.Bases;

public abstract class BaseRepository<TEntity, TContext, TId> : IBaseRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TContext : ApplicationContext
    where TId : struct,
          IComparable,
          IComparable<TId>,
          IConvertible,
          IEquatable<TId>,
          IFormattable
{
    protected readonly TContext Context;
    protected BaseRepository(TContext context)
    {
        Context = context;
    }

    public async Task<TId> AddOrUpdateAsync(TEntity entity)
    {
        if (entity.Id.Equals("0"))
        {
        }
        else
        {

        }
        return entity.Id;
    }

    public async Task BeginTransaction() => Context.Database.BeginTransactionAsync();

    public async Task CommitTransaction() => Context.Database.CommitTransactionAsync();

    public async Task<TId> CreateAsync(TEntity entity)
    {
        await Context.Set<TEntity>().AddAsync(entity);
        await SaveChangesAsync();
        return entity.Id;
    }

    public async Task<bool> DeleteAsync(TId id)
    {
        TEntity entity = await Context.Set<TEntity>().FindAsync(id);
        Context.Set<TEntity>().Remove(entity);
        await Context.SaveChangesAsync();
        return true;
    }

    public async Task<List<TEntity>> GetAsync() => await Context.Set<TEntity>().ToListAsync();

    public async Task<TEntity> GetByIdAsync(TId id) => await Context.Set<TEntity>().FindAsync(id);

    public async Task RollbackTransaction() => await Context.Database.RollbackTransactionAsync();

    public async Task<int> SaveChangesAsync() => await Context.SaveChangesAsync();

    public async Task<TId> UpdateAsync(TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);
        await Context.SaveChangesAsync();
        return await Task.FromResult(entity.Id);
    }
}
