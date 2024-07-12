namespace WebApi.DependencyInjection.Services.Transient;

public class GetGuidTransientService : IGetGuidTransientService
{
    private Guid guid { get; set; }

    public GetGuidTransientService()
    {
        guid = Guid.NewGuid();
    }

    public Guid Execute() => guid;

}