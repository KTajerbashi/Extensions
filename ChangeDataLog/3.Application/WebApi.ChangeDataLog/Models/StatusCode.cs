namespace WebApi.ChangeDataLog.Models;

public class StatusCode
{
    public int NotFound { get => StatusCodes.Status404NotFound; }
    public int InternalServer { get => StatusCodes.Status500InternalServerError; }
    public int NonAuthoritative { get => StatusCodes.Status203NonAuthoritative; }
    public int Ok { get => StatusCodes.Status200OK; }
}
