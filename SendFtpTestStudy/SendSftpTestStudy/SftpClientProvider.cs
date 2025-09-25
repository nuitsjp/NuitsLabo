using Microsoft.Extensions.Options;

namespace SendSftpTestStudy;

/// <summary>
/// SftpClientのインスタンス生成と接続確立を担当するプロバイダー
/// </summary>
public sealed class SftpClientProvider(IOptions<SftpConnectionOptions> options) : ISftpClientProvider
{
    private readonly SftpConnectionOptions _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

    public async Task<SftpClient> CreateAsync(CancellationToken cancellationToken = default)
    {
        var client = new Renci.SshNet.SftpClient(_options.Host, _options.Port, _options.Username, _options.Password);

        if (_options.AcceptAnySshHostKey)
        {
            client.HostKeyReceived += (_, e) => e.CanTrust = true;
        }

        await client.ConnectAsync(cancellationToken);
        return new SftpClient(client);
    }
}
