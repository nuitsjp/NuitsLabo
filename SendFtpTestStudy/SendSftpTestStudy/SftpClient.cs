namespace SendSftpTestStudy;

/// <summary>
/// SFTPサーバーへのファイルアップロード機能を提供するクライアントクラス
/// Renci.SshNet.SftpClientをラップし、DIからの利用を想定した設計とする
/// </summary>
public sealed class SftpClient : IAsyncDisposable
{
    private readonly Renci.SshNet.SftpClient _client;

    internal SftpClient(Renci.SshNet.SftpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    /// <summary>
    /// ファイルをSFTPサーバーへアップロードする
    /// </summary>
    public async Task UploadAsync(
        string remotePath,
        Stream content,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(remotePath))
        {
            throw new ArgumentException("Remote path is required.", nameof(remotePath));
        }

        if (content is null)
        {
            throw new ArgumentNullException(nameof(content));
        }

        cancellationToken.ThrowIfCancellationRequested();

        if (!_client.IsConnected)
        {
            throw new InvalidOperationException("SFTPクライアントが未接続です。");
        }

        var normalizedPath = NormalizeFilePath(remotePath);
        var directory = GetDirectoryPath(normalizedPath);

        if (!string.IsNullOrEmpty(directory))
        {
            EnsureDirectoryExists(_client, directory);
        }

        ResetPosition(content);

        await Task.Run(
            () => _client.UploadFile(content, normalizedPath, true, _ => cancellationToken.ThrowIfCancellationRequested()),
            cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// SFTPクライアントを切断して破棄する
    /// </summary>
    public ValueTask DisposeAsync()
    {
        if (_client.IsConnected)
        {
            _client.Disconnect();
        }

        _client.Dispose();
        return ValueTask.CompletedTask;
    }

    private static void ResetPosition(Stream stream)
    {
        if (stream.CanSeek)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }
    }

    private static void EnsureDirectoryExists(Renci.SshNet.SftpClient client, string remoteDirectory)
    {
        var segments = SplitSegments(remoteDirectory).ToArray();
        if (!segments.Any())
        {
            return;
        }

        var current = "/";
        foreach (var segment in segments)
        {
            current = current == "/" ? $"/{segment}" : $"{current}/{segment}";
            if (!client.Exists(current))
            {
                client.CreateDirectory(current);
            }
        }
    }

    private static string NormalizeFilePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return "/";
        }

        var normalized = path.Replace("\\", "/");
        return normalized.StartsWith("/", StringComparison.Ordinal) ? normalized : "/" + normalized;
    }

    private static string GetDirectoryPath(string path)
    {
        var normalized = NormalizeFilePath(path);
        var lastSlash = normalized.LastIndexOf("/", StringComparison.Ordinal);
        if (lastSlash <= 0)
        {
            return string.Empty;
        }

        return normalized[..lastSlash];
    }

    private static IEnumerable<string> SplitSegments(string path)
    {
        return path
            .Replace("\\", "/")
            .Split("/", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
}
