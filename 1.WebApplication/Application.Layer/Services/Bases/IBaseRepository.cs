using Application.Layer.Model.Base;

namespace Application.Layer.Services.Bases;

public interface IBaseRepository<T, TContext, TId>
    where T : AggregateRoot<TId>
    where TId : struct,
          IComparable,
          IComparable<TId>,
          IConvertible,
          IEquatable<TId>,
          IFormattable
{
    T Insert(T entity);
    T Update(T entity);
    bool Delete(T entity);
    T Get(T entity);
    List<T> GetAll();

    void BeginTransaction();
    void CommitTransaction();
    Task CommitTransactionAsync();
    void RollbackTransaction();
    Task RollbackTransactionAsync();
    void SaveChanges();
    Task SaveChangesAsync();

}
