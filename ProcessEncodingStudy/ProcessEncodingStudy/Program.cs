using System.Diagnostics;
using System.Text;

// winget.exe を起動し、その標準出力/標準エラーをコンソールへそのまま中継します。
try
{
    // Console の現在のエンコーディングに合わせて子プロセス側のエンコーディングを適用
    var psi = new ProcessStartInfo
    {
        FileName = "winget.exe",
        UseShellExecute = false,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        StandardOutputEncoding = Console.OutputEncoding,
        StandardErrorEncoding = Console.OutputEncoding,
        CreateNoWindow = false,
    };

    using var proc = new Process { StartInfo = psi, EnableRaisingEvents = false };

    proc.Start();

    // 非同期で読み取りしながらコンソールへ書き出す（行単位ではなくバッファで転送）
    static async Task PumpAsync(TextReader reader, TextWriter writer)
    {
        var buffer = new char[4096];
        int read;
        while ((read = await reader.ReadAsync(buffer.AsMemory(0, buffer.Length))) > 0)
        {
            await writer.WriteAsync(buffer.AsMemory(0, read));
            await writer.FlushAsync();
        }
    }

    var stdOutTask = Task.Run(() => PumpAsync(proc.StandardOutput, Console.Out));
    var stdErrTask = Task.Run(() => PumpAsync(proc.StandardError, Console.Error));

    proc.WaitForExit();
    Task.WaitAll(stdOutTask, stdErrTask);

    // 親プロセスの終了コードとして winget の終了コードを返す
    Environment.Exit(proc.ExitCode);
}
catch (System.ComponentModel.Win32Exception ex)
{
    Console.Error.WriteLine($"winget.exe の起動に失敗しました: {ex.Message}");
    Environment.Exit(1);
}
catch (Exception ex)
{
    Console.Error.WriteLine($"エラーが発生しました: {ex}");
    Environment.Exit(1);
}
