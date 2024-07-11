using Extensions.DependencyInjection.Abstractions;

namespace WebApi.DependencyInjection.Services;

public interface IGetGuidScopeService : IScopeLifetime
{
    Guid Execute();
}
