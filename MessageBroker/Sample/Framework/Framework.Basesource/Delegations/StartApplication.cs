using Framework.Basesource.Extensions;

namespace Framework.Basesource.Delegations;
public static class StartApplication
{

    public static async Task RunAsync(string applicationName, ApplicationType applicationType, Func<Task> action)
    {
        applicationType.Print($"[x] {applicationType} ~> [{DateTime.Now:G}] Start Application {applicationName} ...");
        await action();
        applicationType.Print($"[x] {applicationType} ~> [{DateTime.Now:G}] Finished Application {applicationName} ...");

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }

    public static void Run(string applicationName, ApplicationType applicationType, Action action)
    {
        applicationType.Print($"[x] {applicationType} ~> [{DateTime.Now:G}] Start Application {applicationName} ...");
        action();
        applicationType.Print($"[x] {applicationType} ~> [{DateTime.Now:G}] Finished Application {applicationName} ...");

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}

