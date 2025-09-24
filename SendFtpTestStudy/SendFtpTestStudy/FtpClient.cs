using FluentFTP;
using Renci.SshNet;

namespace SendFtpTestStudy;

public sealed class FtpClient
{
    public Task UploadAsync(
        FtpConnectionOptions options,
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

        return options.Protocol switch
        {
            FtpProtocol.Ftp => UploadViaFtpAsync(options, remotePath, content, cancellationToken),
            FtpProtocol.Sftp => UploadViaSftpAsync(options, remotePath, content, cancellationToken),
            _ => throw new NotSupportedException($"Unsupported protocol: {options.Protocol}.")
        };
    }

    public Task<byte[]> DownloadAsync(
        FtpConnectionOptions options,
        string remotePath,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (string.IsNullOrWhiteSpace(remotePath))
        {
            throw new ArgumentException("Remote path is required.", nameof(remotePath));
        }

        return options.Protocol switch
        {
            FtpProtocol.Ftp => DownloadViaFtpAsync(options, remotePath, cancellationToken),
            FtpProtocol.Sftp => DownloadViaSftpAsync(options, remotePath, cancellationToken),
            _ => throw new NotSupportedException($"Unsupported protocol: {options.Protocol}.")
        };
    }

    public Task<IReadOnlyList<string>> ListAsync(
        FtpConnectionOptions options,
        string? remoteDirectory,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);

        remoteDirectory ??= string.Empty;

        return options.Protocol switch
        {
            FtpProtocol.Ftp => ListViaFtpAsync(options, remoteDirectory, cancellationToken),
            FtpProtocol.Sftp => ListViaSftpAsync(options, remoteDirectory, cancellationToken),
            _ => throw new NotSupportedException($"Unsupported protocol: {options.Protocol}.")
        };
    }

    private async Task UploadViaFtpAsync(
        FtpConnectionOptions options,
        string remotePath,
        Stream content,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var normalizedPath = NormalizeFilePath(remotePath);

        await using var ftp = CreateAsyncFtpClient(options);
        await ftp.Connect(cancellationToken).ConfigureAwait(false);
        try
        {
            var directory = GetDirectoryPath(normalizedPath);
            if (!string.IsNullOrEmpty(directory))
            {
                var directoryPath = NormalizeDirectory(directory).TrimEnd('/');
                await ftp.CreateDirectory(directoryPath, true, cancellationToken).ConfigureAwait(false);
            }

            ResetPosition(content);
            await ftp.UploadStream(content, normalizedPath, FtpRemoteExists.Overwrite, false, null, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            await ftp.Disconnect(cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task<byte[]> DownloadViaFtpAsync(
        FtpConnectionOptions options,
        string remotePath,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await using var ftp = CreateAsyncFtpClient(options);
        await ftp.Connect(cancellationToken).ConfigureAwait(false);
        try
        {
            var normalizedPath = NormalizeFilePath(remotePath);
            var data = await ftp.DownloadBytes(normalizedPath, cancellationToken).ConfigureAwait(false);
            return data ?? [];
        }
        finally
        {
            await ftp.Disconnect(cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task<IReadOnlyList<string>> ListViaFtpAsync(
        FtpConnectionOptions options,
        string remoteDirectory,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await using var ftp = CreateAsyncFtpClient(options);
        await ftp.Connect(cancellationToken).ConfigureAwait(false);
        try
        {
            var normalizedDirectory = NormalizeDirectory(remoteDirectory);
            var listing = await ftp.GetListing(normalizedDirectory, FtpListOption.AllFiles, cancellationToken).ConfigureAwait(false);
            return listing
                .Where(item => item.Type is FtpObjectType.File or FtpObjectType.Directory)
                .Select(item => item.Name)
                .ToList();
        }
        finally
        {
            await ftp.Disconnect(cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task UploadViaSftpAsync(
        FtpConnectionOptions options,
        string remotePath,
        Stream content,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var client = CreateSftpClient(options);
        await client.ConnectAsync(cancellationToken);

        var normalizedPath = NormalizeFilePath(remotePath);
        var directory = GetDirectoryPath(normalizedPath);
        EnsureSftpDirectoryExists(client, directory);

        ResetPosition(content);
        await Task.Run(
            () => client.UploadFile(content, normalizedPath, true, _ => cancellationToken.ThrowIfCancellationRequested()),
            cancellationToken).ConfigureAwait(false);

        client.Disconnect();
    }

    private async Task<byte[]> DownloadViaSftpAsync(
        FtpConnectionOptions options,
        string remotePath,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var client = CreateSftpClient(options);
        await client.ConnectAsync(cancellationToken);

        var normalizedPath = NormalizeFilePath(remotePath);
        using var buffer = new MemoryStream();
        await Task.Run(
            () => client.DownloadFile(normalizedPath, buffer, _ => cancellationToken.ThrowIfCancellationRequested()),
            cancellationToken).ConfigureAwait(false);

        client.Disconnect();
        return buffer.ToArray();
    }

    private Task<IReadOnlyList<string>> ListViaSftpAsync(
        FtpConnectionOptions options,
        string remoteDirectory,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var client = CreateSftpClient(options);
        client.Connect();

        var normalizedDirectory = NormalizeDirectory(remoteDirectory);
        var entries = client.ListDirectory(normalizedDirectory)
            .Where(entry => entry.Name is not "." and not "..")
            .Select(entry => entry.Name)
            .ToList()
            .AsReadOnly();

        client.Disconnect();
        return Task.FromResult<IReadOnlyList<string>>(entries);
    }

    private static void EnsureSftpDirectoryExists(SftpClient client, string remoteDirectory)
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

    private static AsyncFtpClient CreateAsyncFtpClient(FtpConnectionOptions options)
    {
        var ftp = new AsyncFtpClient(options.Host, options.Username, options.Password, options.Port);
        ftp.Config.DataConnectionType = FtpDataConnectionType.AutoPassive;
        if (options.AcceptAnyCertificate)
        {
            ftp.Config.ValidateAnyCertificate = true;
        }

        return ftp;
    }

    private static SftpClient CreateSftpClient(FtpConnectionOptions options)
    {
        var client = new SftpClient(options.Host, options.Port, options.Username, options.Password);
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

    private static string NormalizeFilePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return "/";
        }

        var normalized = path.Replace('\\', '/');
        return normalized.StartsWith('/') ? normalized : $"/{normalized}";
    }

    private static string NormalizeDirectory(string path)
    {
        var normalized = NormalizeFilePath(path);
        return normalized.EndsWith('/') ? normalized : normalized + "/";
    }

    private static string GetDirectoryPath(string path)
    {
        var normalized = NormalizeFilePath(path);
        var lastSlash = normalized.LastIndexOf('/');
        if (lastSlash <= 0)
        {
            return string.Empty;
        }

        return normalized[..lastSlash];
    }

    private static IEnumerable<string> SplitSegments(string path)
    {
        return path
            .Replace('\\', '/')
            .Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
}
