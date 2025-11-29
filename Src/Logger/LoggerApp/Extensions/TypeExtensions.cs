namespace LoggerApp.Extensions;

public class ConstItem
{
    public string Title { get; set; } = string.Empty;
    public object? Value { get; set; }
}

public static class TypeExtensions
{
    public static List<ConstItem> GetConstFields(this Type type)
    {
        return type.GetFields(System.Reflection.BindingFlags.Public |
                              System.Reflection.BindingFlags.Static |
                              System.Reflection.BindingFlags.FlattenHierarchy)
                   .Where(fi => fi.IsLiteral && !fi.IsInitOnly)  // Only const fields
                   .Select(fi => new ConstItem
                   {
                       Title = fi.Name,
                       Value = fi.GetRawConstantValue()
                   })
                   .ToList();
    }
}
