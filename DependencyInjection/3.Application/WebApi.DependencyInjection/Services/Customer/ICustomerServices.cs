using Extensions.DependencyInjection.Abstractions;
using System;
using WebApi.DependencyInjection.DatabaseContext;

namespace WebApi.DependencyInjection.Services.Customer;

public interface IScopeServices<T> : IScopeLifetime
{
    void Initialize();
    void Create(T value);
    void Update(T value, int index);
    void Remove(int index);
    void Remove(T value);
    List<T> Get();
    T GetIndex(int index);

}
public class CustomerServices : IScopeServices<string>
{
    private readonly IGetStringScopeService _getStringScopeService;
        
    public CustomerServices(IGetStringScopeService getStringScopeService)
    {
        _getStringScopeService = getStringScopeService;
    }

    public void Create(string message)
    {
         DatabaseContextApplication<string>.Add(_getStringScopeService.Execute(message));
    }

    public List<string> Get()
    {
        return DatabaseContextApplication<string>.Get();
    }

    public string GetIndex(int index)
    {
        return DatabaseContextApplication<string>.Get(index);
    }
    

    public void Initialize()
    {
    }

    public void Remove(int index)
    {
        DatabaseContextApplication<string>.Remove(index);
    }

    public void Remove(string value)
    {
    }

    public void Update(string value, int index)
    {
        DatabaseContextApplication<string>.Update(value,index);
    }
}