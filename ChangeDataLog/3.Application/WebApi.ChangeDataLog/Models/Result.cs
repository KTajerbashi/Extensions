namespace WebApi.ChangeDataLog.Models;
public class Result<TData>
{
    public Result()
    {
        
    }
    public Result(TData data, StatusCode statusCode)
    {
        Data = data;
        StatusCode = statusCode;
        Message = "";
    }
    public Result(StatusCode statusCode)
    {
        StatusCode = statusCode;
    }
    public Result(string message,StatusCode statusCode)
    {
        Message = message;
        StatusCode = statusCode;
    }
    public TData Data { get; set; }
    public StatusCode StatusCode { get; set; }
    public string Message { get; set; }
}
