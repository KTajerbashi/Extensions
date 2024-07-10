using WebApi.ChangeDataLog.Models.Bases;

namespace WebApi.ChangeDataLog.Repositories.Bases;

public interface IBaseRepository<TEntity,TId>: IUnitOfWork
    where TEntity : Entity<TId>
    where TId : struct,
          IComparable,
          IComparable<TId>,
          IConvertible,
          IEquatable<TId>,
          IFormattable
{
    Task<TId> AddOrUpdateAsync(TEntity entity);
    Task<TId> CreateAsync(TEntity entity);
    Task<bool> DeleteAsync(TId id);
    Task<TId> UpdateAsync(TEntity entity);

    Task<List<TEntity>> GetAsync();
    Task<TEntity> GetByIdAsync(TId id);
}