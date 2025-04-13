
using HelloWorld.Sender;

Console.WriteLine("=========================STARTED=========================");
await Producer.StartAsync();
Console.WriteLine("=========================FINISHED=========================");
Console.WriteLine("Press Any Key ...");
Console.ReadLine();