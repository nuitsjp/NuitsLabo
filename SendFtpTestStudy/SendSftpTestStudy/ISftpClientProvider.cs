namespace SendSftpTestStudy;

/// <summary>
/// SftpClientの生成を担うファクトリーインターフェース
/// </summary>
public interface ISftpClientProvider
{
    /// <summary>
    /// SFTP接続を確立したSftpClientを作成する
    /// </summary>
    Task<SftpClient> CreateAsync(CancellationToken cancellationToken = default);
}
