using Extensions.DependencyInjection.Abstractions;

namespace WebApi.DependencyInjection.Services.Scope;

public interface IGetGuidScopeService : IScopeLifetime
{
    Guid Execute();
}
