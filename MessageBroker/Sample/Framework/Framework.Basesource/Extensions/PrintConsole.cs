namespace Framework.Basesource.Extensions;

public enum ApplicationType : byte
{
    Receiver = 1,
    Sender = 2,
}

public static class PrintConsole
{
    public static void Print(this ApplicationType type,string message)
    {
        if (type == ApplicationType.Sender)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.WriteLine(message);
        }
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.Black;
    }
}
