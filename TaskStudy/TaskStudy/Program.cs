using CancellationTokenSource source = new CancellationTokenSource();
var token = source.Token;
source.Cancel();

var task = Task.Run(async delegate
{
    try
    {
        await Task.Delay(TimeSpan.FromMinutes(1), token);
        return 42;
    }
    catch (TaskCanceledException)
    {
        Console.WriteLine("タスクがキャンセルされました。");
        throw;
    }
});
source.Cancel();
try
{
    task.Wait();
}
catch (AggregateException ae)
{
    foreach (var e in ae.InnerExceptions)
    {
        Console.WriteLine(e);
    }
}
Console.Write("Task t Status: {0}", task.Status);
