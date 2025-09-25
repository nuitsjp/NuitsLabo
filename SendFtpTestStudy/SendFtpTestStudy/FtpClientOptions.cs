using FluentFTP;

namespace SendFtpTestStudy;

/// <summary>
/// FTP接続に必要な設定情報を格納するデータクラス
/// Microsoft.Extensions.Configurationとの互換性のためパラメータなしコンストラクタを提供
/// </summary>
public sealed class FtpClientOptions
{
    /// <summary>
    /// FTPサーバーのホスト名またはIPアドレス
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// FTPサーバーのポート番号（通常は21、FTPSの場合は990等）
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// FTP認証に使用するユーザー名
    /// </summary>
    public string User { get; set; } = string.Empty;

    /// <summary>
    /// FTP認証に使用するパスワード
    /// セキュリティ上、実際のアプリケーションでは環境変数やSecure Stringを使用することを推奨
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// FTPデータ接続タイプ
    /// AutoPassiveはファイアウォール環境でも動作しやすい（デフォルト）
    /// </summary>
    public FtpDataConnectionType DataConnectionType { get; set; }

    /// <summary>
    /// 接続失敗時の再試行回数（デフォルト: 3回）
    /// </summary>
    public int RetryCount { get; set; } = 3;

    /// <summary>
    /// 再試行間隔（ミリ秒）（デフォルト: 1000ms）
    /// </summary>
    public int RetryInterval { get; set; } = 1000;

    /// <summary>
    /// 接続タイムアウト時間（ミリ秒）（デフォルト: 30秒）
    /// </summary>
    public int ConnectionTimeout { get; set; } = 30000;

    /// <summary>
    /// データ転送のタイムアウト時間（ミリ秒）（デフォルト: 120秒）
    /// </summary>
    public int DataTimeout { get; set; } = 120000;
}
