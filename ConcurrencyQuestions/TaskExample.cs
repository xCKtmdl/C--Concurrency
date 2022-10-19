var myTask = new MyClass();

Task.Run(() => myTask.MyAsync(1, ConsoleColor.Red));
Task.Run(() => myTask.MyAsync(2, ConsoleColor.Green));
Task.Run(() => myTask.MyAsync(3, ConsoleColor.Blue));



public class MyClass
{
    
    public async Task MyAsync(int threadNumber, ConsoleColor color)
    {
        for (int i = 0; i < 6; i++)
        {
            Print(threadNumber, color);
        }
    }


    public static void Print(int threadNumber, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        string message = String.Empty;
        switch (color)
        {
            case ConsoleColor.Red:
                message = String.Format("thread: {0}, color: red", threadNumber);
                break;

            case ConsoleColor.Green:
                message = String.Format("thread: {0}, color: green", threadNumber);
                break;
            case ConsoleColor.Blue:
                message = String.Format("thread: {0}, color: blue", threadNumber);
                break;

            default:
                break;
        }
        Console.WriteLine(message);
    }
}