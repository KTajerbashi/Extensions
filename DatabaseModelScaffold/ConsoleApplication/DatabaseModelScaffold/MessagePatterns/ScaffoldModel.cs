namespace DatabaseModelScaffold.MessagePatterns;

public class ScaffoldModel
{
    public string Server { get; set; }
    public string Database { get; set; }
    public string UserId { get; set; }
    public string Password { get; set; }
    public bool MultipleActiveResultSets { get; set; }
    public bool Encrypt { get; set; }
}
