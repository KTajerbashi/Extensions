namespace LoggerWebApi.Exceptions;

// ۱. خطاهای دامین (بیزینس لاجیک)
public class DomainException : BaseException
{
    public DomainException(string message)
        : base(message, "خطایی در عملیات تجاری رخ داده است.", 400) { }
}
