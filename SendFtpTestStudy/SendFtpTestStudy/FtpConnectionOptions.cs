using FluentFTP;

namespace SendFtpTestStudy;

/// <summary>
/// FTP接続に必要な設定情報を格納するイミュータブルなデータクラス
/// プライマリコンストラクタを使用してコンパクトに定義されている
/// </summary>
/// <param name="host">FTPサーバーのホスト名またはIPアドレス</param>
/// <param name="port">FTPサーバーのポート番号（通常21）</param>
/// <param name="user">FTP認証用のユーザー名</param>
/// <param name="password">FTP認証用のパスワード</param>
/// <param name="dataConnectionType">FTPデータ接続タイプ（デフォルト: AutoPassive）</param>
public sealed class FtpConnectionOptions(
    string host,
    int port,
    string user,
    string password,
    FtpDataConnectionType dataConnectionType = FtpDataConnectionType.AutoPassive)
{
    /// <summary>
    /// FTPサーバーのホスト名またはIPアドレス
    /// </summary>
    public string Host { get; } = host ?? throw new ArgumentNullException(nameof(host));

    /// <summary>
    /// FTPサーバーのポート番号（通常は21、FTPSの場合は990等）
    /// </summary>
    public int Port { get; } = port;

    /// <summary>
    /// FTP認証に使用するユーザー名
    /// </summary>
    public string User { get; } = user ?? throw new ArgumentNullException(nameof(user));

    /// <summary>
    /// FTP認証に使用するパスワード
    /// セキュリティ上、実際のアプリケーションでは環境変数やSecure Stringを使用することを推奨
    /// </summary>
    public string Password { get; } = password ?? throw new ArgumentNullException(nameof(password));

    /// <summary>
    /// FTPデータ接続タイプ
    /// AutoPassiveはファイアウォール環境でも動作しやすい（デフォルト）
    /// </summary>
    public FtpDataConnectionType DataConnectionType { get; } = dataConnectionType;
}
