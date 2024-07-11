using Extensions.DependencyInjection.Abstractions;

namespace WebApi.DependencyInjection.Services;

public interface IGetGuidSingletonService : ISingletoneLifetime
{
    Guid Execute();
}