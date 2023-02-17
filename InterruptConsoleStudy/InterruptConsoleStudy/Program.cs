using System.Runtime.InteropServices;

if (TryReadLine(TimeSpan.FromSeconds(3), out var line))
{
    Console.WriteLine($"ReadLine was completed. : {line}");
}
else
{
    Console.WriteLine($"ReadLine was interrupted.");
}


static bool TryReadLine(TimeSpan timeSpan, out string line)
{
    var isRead = false;
    Task.Delay(timeSpan).ContinueWith(_ =>
    {
        // 読み込みが未完了の場合だけ中断する。
        if (!isRead)
        {
            const int standardInputHandle = -10;
            var handle = GetStdHandle(standardInputHandle);
            CancelIoEx(handle, IntPtr.Zero);
        }
    });

    try
    {
        line = Console.ReadLine()!;
        isRead = true;
        return true;
    }
    catch (InvalidOperationException)
    {
    }
    catch (OperationCanceledException)
    {
    }

    line = default!;
    return false;
}

[DllImport("kernel32.dll", SetLastError = true)]
static extern IntPtr GetStdHandle(int nStdHandle);

[DllImport("kernel32.dll", SetLastError = true)]
static extern bool CancelIoEx(IntPtr handle, IntPtr lpOverlapped);

