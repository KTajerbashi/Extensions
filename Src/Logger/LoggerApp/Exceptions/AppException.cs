namespace LoggerApp.Exceptions;

// ۲. خطاهای اپلیکیشن (خطاهای سرویس)
public class AppException : BaseException
{
    public AppException(string message)
        : base(message, "مشکلی در پردازش درخواست پیش آمد.", 422) { }
}
