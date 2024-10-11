Console.WriteLine("Doing...");
await DoAsync();
Console.WriteLine("Completed!");

static async Task DoAsync()
{
    await Task.Run(() =>
    {
        // 重たい同期処理
        Thread.Sleep(TimeSpan.FromSeconds(3));
    });
}

static Task HeavyComputationAsync()
{
    return Task.CompletedTask;
}

