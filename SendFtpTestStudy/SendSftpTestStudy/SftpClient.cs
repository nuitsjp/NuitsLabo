using System.Linq;
using Renci.SshNet;

namespace SendSftpTestStudy;

public sealed class SftpClient
{
    public async Task UploadAsync(
        SftpConnectionOptions options,
        string remotePath,
        Stream content,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(content);

        if (string.IsNullOrWhiteSpace(remotePath))
        {
            throw new ArgumentException("Remote path is required.", nameof(remotePath));
        }

        cancellationToken.ThrowIfCancellationRequested();

        using var client = CreateSftpClient(options);
        client.Connect();

        try
        {
            var normalizedPath = NormalizeFilePath(remotePath);
            var directory = GetDirectoryPath(normalizedPath);
            EnsureDirectoryExists(client, directory);

            ResetPosition(content);
            await Task.Run(
                () => client.UploadFile(content, normalizedPath, true, _ => cancellationToken.ThrowIfCancellationRequested()),
                cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            if (client.IsConnected)
            {
                client.Disconnect();
            }
        }
    }

    private static Renci.SshNet.SftpClient CreateSftpClient(SftpConnectionOptions options)
    {
        var client = new Renci.SshNet.SftpClient(options.Host, options.Port, options.Username, options.Password);
        if (options.AcceptAnySshHostKey)
        {
            client.HostKeyReceived += (_, e) => e.CanTrust = true;
        }

        return client;
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
        var segments = SplitSegments(remoteDirectory);
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



