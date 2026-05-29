namespace LoggerApp.Exceptions;

// ۴. خطاهای ای‌پی‌آی (خارجی)
public class ApiException : BaseException
{
    public ApiException(string message, int statusCode = 502)
        : base(message, "خطا در برقراری ارتباط با سرویس جانبی.", statusCode) { }
}
