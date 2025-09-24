namespace SendFtpTestStudy;

/// <summary>
/// FTP接続に必要な設定情報を格納するイミュータブルなデータクラス
/// プライマリコンストラクタを使用してコンパクトに定義されている
/// </summary>
/// <param name="host">FTPサーバーのホスト名またはIPアドレス</param>
/// <param name="port">FTPサーバーのポート番号（通常21）</param>
/// <param name="username">FTP認証用のユーザー名</param>
/// <param name="password">FTP認証用のパスワード</param>
/// <param name="acceptAnyCertificate">SSL証明書の検証を無効にするかどうか（テスト環境用）</param>
public sealed class FtpConnectionOptions(
    string host,
    int port,
    string username,
    string password,
    bool acceptAnyCertificate = false)
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
    public string Username { get; } = username ?? throw new ArgumentNullException(nameof(username));

    /// <summary>
    /// FTP認証に使用するパスワード
    /// セキュリティ上、実際のアプリケーションでは環境変数やSecure Stringを使用することを推奨
    /// </summary>
    public string Password { get; } = password ?? throw new ArgumentNullException(nameof(password));

    /// <summary>
    /// SSL証明書の検証を無効にするかどうか
    /// trueの場合、自己署名証明書や無効な証明書も受け入れる（テスト環境専用）
    /// 本番環境では必ずfalseにして適切な証明書検証を行うべき
    /// </summary>
    public bool AcceptAnyCertificate { get; } = acceptAnyCertificate;
}
