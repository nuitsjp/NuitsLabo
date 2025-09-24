using FluentFTP;

namespace SendFtpTestStudy;

/// <summary>
/// FtpClientの生成と接続管理を行うプロバイダークラス
/// FTP接続を確立してからFtpClientインスタンスを提供する
/// </summary>
public sealed class FtpClientProvider
{
    /// <summary>
    /// FTP接続を確立してFtpClientを作成する
    /// </summary>
    /// <param name="options">FTP接続に必要な設定情報</param>
    /// <param name="cancellationToken">キャンセル処理用のトークン</param>
    /// <returns>接続済みのFtpClientインスタンス</returns>
    public static async Task<FtpClient> CreateAsync(
        FtpConnectionOptions options, 
        CancellationToken cancellationToken = default)
    {
        // AsyncFtpClientを作成し、基本設定を行う
        // FTPクライアントを接続情報で初期化
        var ftp = new AsyncFtpClient(options.Host, options.Username, options.Password, options.Port);

        // データ接続タイプを設定
        ftp.Config.DataConnectionType = options.DataConnectionType;

        // 任意の証明書を受け入れる（信頼された環境でしか利用しないため）
        ftp.Config.ValidateAnyCertificate = true;

        var asyncFtpClient = ftp;
        
        // FTP接続を確立
        await asyncFtpClient.Connect(cancellationToken).ConfigureAwait(false);
        
        // 接続済みのFtpClientを作成して返す
        return new FtpClient(asyncFtpClient);
    }
}