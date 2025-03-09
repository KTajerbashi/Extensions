namespace Serializers.WebApi.DependencyInjections;
public interface ISendEmail
{
    string SendMail();
}

public class SendSingleton : ISendEmail
{
    int Count = 0;
    public Guid Code { get; set; }
    public SendSingleton()
    {
        Count++;
        Console.WriteLine($"SendSingleton : Count {Count}");
        Code = Guid.NewGuid();
    }
    public string SendMail()
    {
        return Code.ToString();
    }
}
public class SendScope : ISendEmail
{
    int Count = 0;
    public Guid Code { get; set; }
    public SendScope()
    {
        Count++;
        Console.WriteLine($"SendScope : Count {Count} , {this.GetHashCode()}");
        Code = Guid.NewGuid();
    }
    public string SendMail()
    {
        return Code.ToString();
    }
}
public class SendTransient : ISendEmail
{
    int Count = 0;
    public Guid Code { get; set; }
    public SendTransient()
    {
        Count++;
        Console.WriteLine($"SendTransient : Count {Count} , {this.GetHashCode()}");
        Code = Guid.NewGuid();
    }
    public string SendMail()
    {
        return Code.ToString();
    }
}