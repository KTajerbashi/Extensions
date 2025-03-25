namespace Serializers.WebApi.Exceptions;

public class AppException : Exception
{
    public AppException(string message) : base(message)
    {
    }
    public AppException(Exception exception, string message) : base(message, exception)
    {

    }
}


public class InvalidMainDatasourceException : Exception
{
    public string MSG { get; set; } = string.Empty;
    public InvalidMainDatasourceException(string message) : base(message)
    {
        MSG = $"{DateTime.Now} => {message}";
    }
    public InvalidMainDatasourceException(Exception exception, string message) : base(message, exception)
    {

    }
}


public class InvalidDatasourceException : InvalidMainDatasourceException
{
    public InvalidDatasourceException(string message) : base(message)
    {
    }
}

public class InvalidTableException : InvalidMainDatasourceException
{
    public InvalidTableException(Exception exception, string message) : base(exception, message)
    {
    }
}


public class InvalidColumnException : InvalidMainDatasourceException
{
    public InvalidColumnException(Exception exception, string message) : base(exception, message)
    {
    }
}
