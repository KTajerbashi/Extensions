namespace LoggerApp.Exceptions;

// ۳. خطاهای دیتابیس (زیرساخت)
public class DataBaseException : BaseException
{
    public DataBaseException(string message)
        : base(message, "خطای ارتباط با پایگاه داده.", 500) { }
}
