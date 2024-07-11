using Extensions.DependencyInjection.Abstractions;

namespace WebApi.DependencyInjection.Services;

public interface IGetGuidTransientService : ITransientLifetime
{
    Guid Execute();
}