namespace WebApi.DependencyInjection.Services.Customer;

public class GetStringScopeService : IGetStringScopeService
{
    public GetStringScopeService()
    {
    }

    public string Execute(string message) => $"GetStringScopeService : {message}";
}
