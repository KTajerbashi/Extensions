using Extensions.DependencyInjection.Abstractions;

namespace WebApi.DependencyInjection.Services.Customer;

public interface IGetStringScopeService : IScopeLifetime
{
    string Execute(string message);
}
