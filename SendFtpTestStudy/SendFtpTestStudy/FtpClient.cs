using FluentFTP;

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

        return UploadViaFtpAsync(options, remotePath, content, cancellationToken);
    }

    private static async Task UploadViaFtpAsync(
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

        var normalized = path.Replace("\\", "/");
        return normalized.StartsWith("/", StringComparison.Ordinal) ? normalized : "/" + normalized;
    }

    private static string NormalizeDirectory(string path)
    {
        var normalized = NormalizeFilePath(path);
        return normalized.EndsWith("/", StringComparison.Ordinal) ? normalized : normalized + "/";
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
}
