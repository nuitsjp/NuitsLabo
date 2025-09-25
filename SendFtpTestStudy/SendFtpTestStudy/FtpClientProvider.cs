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

        Exception? lastException = null;
        
        // 手動リトライロジック実装（Pollyライブラリよりもシンプル）
        for (int attempt = 0; attempt <= optionsValue.RetryCount; attempt++)
        {
            try
            {
                // CancellationTokenSourceでタイムアウト制御
                using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                timeoutCts.CancelAfter(optionsValue.ConnectionTimeout);

                // AsyncFtpClientを作成し、基本設定を行う
                var ftp = new AsyncFtpClient(optionsValue.Host, optionsValue.User, optionsValue.Password, optionsValue.Port);

                // データ接続タイプを設定
                ftp.Config.DataConnectionType = optionsValue.DataConnectionType;

                // タイムアウト設定を適用
                ftp.Config.ConnectTimeout = optionsValue.ConnectionTimeout;
                ftp.Config.DataConnectionConnectTimeout = optionsValue.DataTimeout;
                ftp.Config.DataConnectionReadTimeout = optionsValue.DataTimeout;

                // 任意の証明書を受け入れる（信頼された環境でしか利用しないため）
                ftp.Config.ValidateAnyCertificate = true;

                // FTP接続を確立
                await ftp.Connect(timeoutCts.Token).ConfigureAwait(false);

                // 接続成功時は接続済みのFtpClientを作成して返す
                return new FtpClient(ftp);
            }
            catch (Exception ex) when (attempt < optionsValue.RetryCount)
            {
                // 最後の試行でない場合は例外を保存してリトライ
                lastException = ex;
                
                // 再試行間隔を待機
                if (optionsValue.RetryInterval > 0)
                {
                    await Task.Delay(optionsValue.RetryInterval, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        // すべてのリトライが失敗した場合は最後の例外を再スロー
        throw lastException ?? new InvalidOperationException("Connection failed after all retry attempts.");
    }
}