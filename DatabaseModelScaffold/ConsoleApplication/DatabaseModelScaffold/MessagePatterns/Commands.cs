namespace DatabaseModelScaffold.MessagePatterns;

public static class Commands
{
    public static string RunScaffoldCommand(ScaffoldModel model)
    {
        return $@"
Scaffold-DbContext
""
SERVER = {model.Server};
DATABASE = {model.Database};
USER ID = {model.UserId};
PASSWORD = {model.Password};
MultipleActiveResultSets = {model.MultipleActiveResultSets};
Encrypt = {model.Encrypt};
""
Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
";
    }
}
