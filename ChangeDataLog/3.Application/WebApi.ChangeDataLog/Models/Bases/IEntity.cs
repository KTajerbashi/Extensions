namespace WebApi.ChangeDataLog.Models.Bases;

public interface IEntity
{
}
public abstract class Entity<TId> : IEntity
{
    public TId Id { get; set; }
}

public abstract class Entity : Entity<long>
{

}