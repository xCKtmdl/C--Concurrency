# C# Concurrency Interview Questions

Here is a simple concurrency question I was asked in a job interview.

You're presented with the following code and asked what is the implied intention.

```csharp

var thread1 = new Thread(() => NewClass.MyThread(1, ConsoleColor.Red));
thread1.Start();

var thread2 = new Thread(() => NewClass.MyThread(2, ConsoleColor.Green));
thread2.Start();

var thread3 = new Thread(() => NewClass.MyThread(3, ConsoleColor.Blue));
thread3.Start();



public static class NewClass
{
    public static void MyThread(int threadNumber, ConsoleColor color)
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
```

So apprently what it is supposed to do is fire off these threads with thread
id's 1, 2, and 3 and print the appropriate messages with the correct foreground
color in the appropriate groups of text.

Instead what happens is these threads are not run in synchronously (one after
 the other, but instead essentially run asynchronously/concurrently/simulatenously
  i.e. at the same time and a race condition occurrs with all these threads fighting
  over the access to the shared resource which in this case is the foreground color.

So here is the indeterministic output we get:

![thread-1.jpf](/doc/img/thread-1.jpg)

You see we at least get the correct number of messages, 6, for each thread
 and also the correct text "red", "green", and "blue" for each of the thread
id numbers.

The next logical step is to surround the code accessing the foreground resource
with a object lock:

```csharp
    private static object myLock = new object();
    public static void MyThread(int threadNumber, ConsoleColor color)
    {
        lock (myLock)
        {
            for (int i = 0; i < 6; i++)
            {
                Print(threadNumber, color);
            }
        }
        
    }
```

Now we see from the output that the foreground color is getting set in the
correct threads, but the threads still get fired off and we don't really
know in what order they'll get executed:

![thread-2.jpf](/doc/img/thread-2.jpg)

If you wanted to run the threads synchronously, you could get rid of the object
lock and use the Join() method as follows:

```csharp
var thread1 = new Thread(() => NewClass.MyThread(1, ConsoleColor.Red));
thread1.Start();
thread1.Join();

var thread2 = new Thread(() => NewClass.MyThread(2, ConsoleColor.Green));
thread2.Start();
thread2.Join();

var thread3 = new Thread(() => NewClass.MyThread(3, ConsoleColor.Blue));
thread3.Start();
thread3.Join();
```

![thread-3.jpf](/doc/img/thread-3.jpg)