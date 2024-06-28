using Microsoft.EntityFrameworkCore;
using WebApplicationAPI.DataAccess.ChangeDataLog;
using WebApplicationAPI.Model.Base;

namespace WebApplicationAPI.Services.Bases;

public abstract class BaseRepository<T, TContext, TId>
    : IBaseRepository<T, TContext, TId>
    where T : AggregateRoot<TId>
    where TContext : DatabaseContext
    where TId : struct,
          IComparable,
          IComparable<TId>,
          IConvertible,
          IEquatable<TId>,
          IFormattable
{
    protected readonly TContext _context;

    public BaseRepository(TContext dbContext)
    {
        _context = dbContext;
    }

    public void BeginTransaction()
    {
        _context.Database.BeginTransaction();
    }

    public void CommitTransaction()
    {
        _context.Database.CommitTransaction();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public bool Delete(T entity)
    {
        _context.Remove(entity);
        return true;
    }

    public T Get(T entity)
    {
        return _context.Set<T>().Find(entity.Id);
    }

    public List<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public T Insert(T entity)
    {
        _context.Set<T>().Add(entity);
        return entity;
    }

    public void RollbackTransaction()
    {
        _context.Database.RollbackTransaction();
    }

    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        return entity;
    }
}
