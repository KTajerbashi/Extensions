namespace Extensions.UsersManagement.Abstractions;

public sealed class KeyValue<TKey, TValue>
{
    public TKey Key { get; set; }
    public TValue Value { get; set; }
    public string Title { get; set; }
}