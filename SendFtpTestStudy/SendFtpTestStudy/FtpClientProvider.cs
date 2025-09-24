using FluentFTP;
using Microsoft.Extensions.Options;

namespace SendFtpTestStudy;

/// <summary>
/// FtpClientの生成と接続管理を行うプロバイダークラス
/// FTP接続を確立してからFtpClientインスタンスを提供する
/// IOptions経由で設定を注入し、Dependency Injectionに対応
/// </summary>
public sealed class FtpClientProvider : IFtpClientProvider
{
    private readonly FtpConnectionOptions _options;

    /// <summary>
    /// FtpClientProviderのコンストラクタ
    /// </summary>
    /// <param name="options">DI経由で注入されるFTP接続設定</param>
    public FtpClientProvider(IOptions<FtpConnectionOptions> options)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// FTP接続を確立してFtpClientを作成する
    /// </summary>
    /// <param name="cancellationToken">キャンセル処理用のトークン</param>
    /// <returns>接続済みのFtpClientインスタンス</returns>
    public async Task<FtpClient> CreateAsync(CancellationToken cancellationToken = default)
    {
        // AsyncFtpClientを作成し、基本設定を行う
        // FTPクライアントを接続情報で初期化
        var ftp = new AsyncFtpClient(_options.Host, _options.Username, _options.Password, _options.Port);

        // データ接続タイプを設定
        ftp.Config.DataConnectionType = _options.DataConnectionType;

        // 任意の証明書を受け入れる（信頼された環境でしか利用しないため）
        ftp.Config.ValidateAnyCertificate = true;

        var asyncFtpClient = ftp;

        // FTP接続を確立
        await asyncFtpClient.Connect(cancellationToken).ConfigureAwait(false);

        // 接続済みのFtpClientを作成して返す
        return new FtpClient(asyncFtpClient);
    }
}