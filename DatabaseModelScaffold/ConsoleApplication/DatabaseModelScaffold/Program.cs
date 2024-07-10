// See https://aka.ms/new-console-template for more information
using DatabaseModelScaffold.MessagePatterns;
using System.Management.Automation.Runspaces;
using System.Text;

Console.WriteLine("==================================");
var result = Commands.RunScaffoldCommand(new ScaffoldModel
{
    Server= "TAJERBASHI",
    Database= "ChangeDataLog_Db",
    UserId="sa",
    Password="123123",
    MultipleActiveResultSets = true,
    Encrypt = false,
});
Console.WriteLine(result);
Runspace runspace = RunspaceFactory.CreateRunspace();
runspace.Open();
Pipeline pipeline = runspace.CreatePipeline();
pipeline.Commands.AddScript(result);
pipeline.Commands.Add("Out-String");
runspace.Close();
StringBuilder stringBuilder = new StringBuilder();
var res = stringBuilder.ToString();
Console.WriteLine("==================================");

