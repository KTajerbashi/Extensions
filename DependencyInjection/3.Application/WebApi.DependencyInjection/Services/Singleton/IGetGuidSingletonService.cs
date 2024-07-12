using Extensions.DependencyInjection.Abstractions;

namespace WebApi.DependencyInjection.Services.Singleton;

public interface IGetGuidSingletonService : ISingletoneLifetime
{
    Guid Execute();
}