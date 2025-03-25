namespace Serializers.WebApi.Models;

public static class ResponseApi
{
    public static Response<T> Success<T>(T data)
    {
        return new Response<T>()
        {
            Data = data,
            Message = "Success Genration !",
            Success = false
        };
    }
    public static Response<T> Faild<T>(T data)
    {
        return new Response<T>()
        {
            Data = data,
            Message = "Faild Genration !",
            Success = false
        };
    }
}

public class Response<T>
{
    public T Data { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
}