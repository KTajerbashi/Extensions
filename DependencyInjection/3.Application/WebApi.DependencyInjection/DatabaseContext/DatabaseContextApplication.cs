namespace WebApi.DependencyInjection.DatabaseContext;

public interface IDatabaseContextApplication<TEntity>
{

}
public static class DatabaseContextApplication<TEntity>
{
    private static List<TEntity> DataList = new List<TEntity>();
    
    public static TEntity Get(int index) => DataList[index];
    public static List<TEntity> Get() => DataList.ToList();
    public static void Add(TEntity value) => DataList.Add(value);
    public static void Update(TEntity value, int index) => DataList[index] = value;
    public static void Remove(int index) => DataList.RemoveAt(index);

}
