using Extensions.DependencyInjection.Abstractions;

namespace WebApi.DependencyInjection.Services.Transient;

public interface IGetGuidTransientService : ITransientLifetime
{
    Guid Execute();
}