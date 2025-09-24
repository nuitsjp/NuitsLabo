namespace SendFtpTestStudy;

/// <summary>
/// FtpClientの生成を行うプロバイダーのインターフェイス
/// Dependency Injectionによるテスタビリティと疎結合を実現する
/// </summary>
public interface IFtpClientProvider
{
    /// <summary>
    /// FTP接続を確立してFtpClientを作成する
    /// </summary>
    /// <param name="cancellationToken">キャンセル処理用のトークン</param>
    /// <returns>接続済みのFtpClientインスタンス</returns>
    Task<FtpClient> CreateAsync(CancellationToken cancellationToken = default);
}