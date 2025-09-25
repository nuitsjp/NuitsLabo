using Microsoft.Extensions.Options;

namespace SendSftpTestStudy;

/// <summary>
/// SftpClientのインスタンス生成と接続確立を担当するプロバイダー
/// </summary>
public sealed class SftpClientProvider : ISftpClientProvider
{
    private readonly SftpConnectionOptions _options;

    public SftpClientProvider(IOptions<SftpConnectionOptions> options)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<SftpClient> CreateAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var client = new Renci.SshNet.SftpClient(_options.Host, _options.Port, _options.Username, _options.Password);

        if (_options.AcceptAnySshHostKey)
        {
            client.HostKeyReceived += (_, e) => e.CanTrust = true;
        }

        try
        {
            await Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                client.Connect();
            }, cancellationToken).ConfigureAwait(false);
        }
        catch
        {
            client.Dispose();
            throw;
        }

        if (!client.IsConnected)
        {
            client.Dispose();
            throw new InvalidOperationException("SFTPサーバーへの接続に失敗しました。");
        }

        return new SftpClient(client);
    }
}
