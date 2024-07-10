namespace WebApi.ChangeDataLog.Repositories.Bases;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
    Task BeginTransaction();
    Task CommitTransaction();
    Task RollbackTransaction();
}
