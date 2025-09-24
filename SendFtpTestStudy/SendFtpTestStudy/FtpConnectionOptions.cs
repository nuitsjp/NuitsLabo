using FluentFTP;

namespace SendFtpTestStudy;

/// <summary>
/// FTP接続に必要な設定情報を格納するデータクラス
/// Microsoft.Extensions.Configurationとの互換性のためパラメータなしコンストラクタを提供
/// </summary>
public sealed class FtpConnectionOptions
{
    /// <summary>
    /// パラメータなしコンストラクタ（設定システム用）
    /// </summary>
    public FtpConnectionOptions()
    {
        Host = string.Empty;
        User = string.Empty;
        Password = string.Empty;
        Port = 21;
        DataConnectionType = FtpDataConnectionType.AutoPassive;
    }

    /// <summary>
    /// 全パラメータ指定コンストラクタ
    /// </summary>
    /// <param name="host">FTPサーバーのホスト名またはIPアドレス</param>
    /// <param name="port">FTPサーバーのポート番号（通常21）</param>
    /// <param name="user">FTP認証用のユーザー名</param>
    /// <param name="password">FTP認証用のパスワード</param>
    /// <param name="dataConnectionType">FTPデータ接続タイプ（デフォルト: AutoPassive）</param>
    public FtpConnectionOptions(
        string host,
        int port,
        string user,
        string password,
        FtpDataConnectionType dataConnectionType = FtpDataConnectionType.AutoPassive)
    {
        Host = host ?? throw new ArgumentNullException(nameof(host));
        Port = port;
        User = user ?? throw new ArgumentNullException(nameof(user));
        Password = password ?? throw new ArgumentNullException(nameof(password));
        DataConnectionType = dataConnectionType;
    }

    /// <summary>
    /// FTPサーバーのホスト名またはIPアドレス
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// FTPサーバーのポート番号（通常は21、FTPSの場合は990等）
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// FTP認証に使用するユーザー名
    /// </summary>
    public string User { get; set; }

    /// <summary>
    /// FTP認証に使用するパスワード
    /// セキュリティ上、実際のアプリケーションでは環境変数やSecure Stringを使用することを推奨
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// FTPデータ接続タイプ
    /// AutoPassiveはファイアウォール環境でも動作しやすい（デフォルト）
    /// </summary>
    public FtpDataConnectionType DataConnectionType { get; set; }
}
