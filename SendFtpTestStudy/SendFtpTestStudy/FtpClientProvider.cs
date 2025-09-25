using FluentFTP;
using Microsoft.Extensions.Options;

namespace SendFtpTestStudy;

/// <summary>
/// FtpClientの生成と接続管理を行うプロバイダークラス
/// FTP接続を確立してからFtpClientインスタンスを提供する
/// IOptions経由で設定を注入し、Dependency Injectionに対応
/// バリデーションはCreateAsync実行時に行われる
/// </summary>
public sealed class FtpClientProvider(IOptions<FtpClientOptions> options) : IFtpClientProvider
{
    /// <summary>
    /// FTP接続を確立してFtpClientを作成する
    /// </summary>
    /// <param name="cancellationToken">キャンセル処理用のトークン</param>
    /// <returns>接続済みのFtpClientインスタンス</returns>
    public async Task<IFtpClient> CreateAsync(CancellationToken cancellationToken = default)
    {
        // options.Valueアクセス時にバリデーションが実行される
        var optionsValue = options.Value;

        // AsyncFtpClientを作成し、基本設定を行う
        // FTPクライアントを接続情報で初期化
        var ftp = new AsyncFtpClient(optionsValue.Host, optionsValue.User, optionsValue.Password, optionsValue.Port);

        // データ接続タイプを設定
        ftp.Config.DataConnectionType = optionsValue.DataConnectionType;

        // 任意の証明書を受け入れる（信頼された環境でしか利用しないため）
        ftp.Config.ValidateAnyCertificate = true;

        // FTP接続を確立
        await ftp.Connect(cancellationToken).ConfigureAwait(false);

        // 接続済みのFtpClientを作成して返す
        return new FtpClient(ftp);
    }
}