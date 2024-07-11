using Extensions.DependencyInjection.Abstractions;

namespace WebApi.DependencyInjection.Services.Customer;

public interface ICustomerServices<T> : IScopeLifetime
{
    void Initialize();
    void Create(T value);
    void Update(T value, int index);
    void Remove(int index);
    void Remove(T value);
    string GetIndex(string value);
    List<T> Get();
    T GetIndex(int index);

}
public class CustomerServices : ICustomerServices<string>
{
    private readonly IGetStringScopeService _getStringScopeService;
    private List<string> datalist;
    public CustomerServices(IGetStringScopeService getStringScopeService)
    {
        datalist = new List<string>();
        _getStringScopeService = getStringScopeService;
    }

    public void Create(string message)
    {
        datalist.Add(_getStringScopeService.Execute(message));
    }

    public List<string> Get()
    {
        return datalist;
    }

    public string GetIndex(int index)
    {
        return datalist[index];
    }
    public string GetIndex(string value)
    {
        return datalist.Where(item => item.Equals(value)).FirstOrDefault();
    }

    public void Initialize()
    {
        datalist = new List<string>();
    }

    public void Remove(int index)
    {
        datalist.RemoveAt(index);
    }

    public void Remove(string value)
    {
        datalist.Remove(value);
    }

    public void Update(string value, int index)
    {
        datalist[index] = value;
    }
}