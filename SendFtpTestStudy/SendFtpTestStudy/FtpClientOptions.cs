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
}
