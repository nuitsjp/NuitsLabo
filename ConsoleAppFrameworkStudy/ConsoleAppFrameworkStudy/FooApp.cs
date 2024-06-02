namespace ConsoleAppFrameworkStudy;

public class FooApp : ConsoleAppBase
{
    [RootCommand]
    public void Hello()
    {
        Console.WriteLine("Hello");
    }
}