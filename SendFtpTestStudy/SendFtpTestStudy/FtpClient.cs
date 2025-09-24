using System.Net;
using System.Text;
using System.Linq;
using Renci.SshNet;

namespace SendFtpTestStudy;

public sealed class FtpClient
{
    private const int DefaultBufferSize = 81920;

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
        var directory = GetDirectoryPath(normalizedPath);
        if (!string.IsNullOrEmpty(directory))
        {
            await EnsureFtpDirectoryExistsAsync(options, directory, cancellationToken).ConfigureAwait(false);
        }

        var request = CreateFtpRequest(options, normalizedPath, WebRequestMethods.Ftp.UploadFile);
        using var registration = cancellationToken.Register(request.Abort);
        await using (var requestStream = await request.GetRequestStreamAsync().ConfigureAwait(false))
        {
            ResetPosition(content);
            await content.CopyToAsync(requestStream, DefaultBufferSize, cancellationToken).ConfigureAwait(false);
        }

        using var response = (FtpWebResponse)await request.GetResponseAsync().ConfigureAwait(false);
        _ = response.StatusCode;
    }

    private async Task<byte[]> DownloadViaFtpAsync(
        FtpConnectionOptions options,
        string remotePath,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var request = CreateFtpRequest(options, remotePath, WebRequestMethods.Ftp.DownloadFile);
        using var registration = cancellationToken.Register(request.Abort);
        using var response = (FtpWebResponse)await request.GetResponseAsync().ConfigureAwait(false);
        await using var responseStream = response.GetResponseStream() ?? Stream.Null;
        using var buffer = new MemoryStream();
        await responseStream.CopyToAsync(buffer, DefaultBufferSize, cancellationToken).ConfigureAwait(false);
        return buffer.ToArray();
    }

    private async Task<IReadOnlyList<string>> ListViaFtpAsync(
        FtpConnectionOptions options,
        string remoteDirectory,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var request = CreateFtpRequest(options, NormalizeDirectory(remoteDirectory), WebRequestMethods.Ftp.ListDirectory);
        using var registration = cancellationToken.Register(request.Abort);
        using var response = (FtpWebResponse)await request.GetResponseAsync().ConfigureAwait(false);
        await using var responseStream = response.GetResponseStream() ?? Stream.Null;
        using var reader = new StreamReader(responseStream, Encoding.UTF8, leaveOpen: true);

        var entries = new List<string>();
        while (!reader.EndOfStream)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var line = await reader.ReadLineAsync().ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(line))
            {
                entries.Add(line.Trim());
            }
        }

        return entries;
    }

    private async Task UploadViaSftpAsync(
        FtpConnectionOptions options,
        string remotePath,
        Stream content,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var client = CreateSftpClient(options);
        client.Connect();

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
        client.Connect();

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

    private async Task EnsureFtpDirectoryExistsAsync(
        FtpConnectionOptions options,
        string remoteDirectory,
        CancellationToken cancellationToken)
    {
        var segments = SplitSegments(remoteDirectory);
        if (!segments.Any())
        {
            return;
        }

        var current = new StringBuilder();
        foreach (var segment in segments)
        {
            if (current.Length > 0)
            {
                current.Append('/');
            }

            current.Append(segment);
            var makeDirRequest = CreateFtpRequest(options, current + "/", WebRequestMethods.Ftp.MakeDirectory);
            using var registration = cancellationToken.Register(makeDirRequest.Abort);
            try
            {
                using var _ = (FtpWebResponse)await makeDirRequest.GetResponseAsync().ConfigureAwait(false);
            }
            catch (WebException ex) when (IsAlreadyExistsResponse(ex))
            {
                // Directory already exists; ignore.
            }
        }
    }

    private static bool IsAlreadyExistsResponse(WebException ex)
    {
        if (ex.Response is FtpWebResponse ftpResponse)
        {
            using (ftpResponse)
            {
                return ftpResponse.StatusCode is FtpStatusCode.ActionNotTakenFileUnavailable or FtpStatusCode.ActionNotTakenFilenameNotAllowed;
            }
        }

        return false;
    }

    private static void EnsureSftpDirectoryExists(SftpClient client, string remoteDirectory)
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

    private static FtpWebRequest CreateFtpRequest(
        FtpConnectionOptions options,
        string remotePath,
        string method)
    {
        var builder = new UriBuilder("ftp", options.Host, options.Port, NormalizePathForUri(remotePath));
        var request = (FtpWebRequest)WebRequest.Create(builder.Uri);
        request.Method = method;
        request.Credentials = new NetworkCredential(options.Username, options.Password);
        request.UseBinary = true;
        request.UsePassive = true;
        request.KeepAlive = false;
        return request;
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

    private static string NormalizePathForUri(string path)
    {
        if (string.IsNullOrWhiteSpace(path) || path == "/")
        {
            return "/";
        }

        var trimmed = path.Replace('\\', '/').TrimStart('/');
        return Uri.EscapeUriString(trimmed);
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
