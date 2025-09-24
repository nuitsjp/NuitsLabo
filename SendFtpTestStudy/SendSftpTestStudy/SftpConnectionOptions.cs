namespace SendSftpTestStudy;

/// <summary>
/// SFTP接続に必要な設定情報を格納するイミュータブルなデータクラス
/// SFTPはSSHプロトコルを使用するため、FTPとは異なるホストキー検証設定が必要
/// プライマリコンストラクタを使用してコンパクトに定義されている
/// </summary>
/// <param name="host">SFTPサーバーのホスト名またはIPアドレス</param>
/// <param name="port">SFTPサーバーのポート番号（通常22）</param>
/// <param name="username">SSH認証用のユーザー名</param>
/// <param name="password">SSH認証用のパスワード</param>
/// <param name="acceptAnySshHostKey">SSHホストキーの検証を無効にするかどうか（テスト環境用）</param>
public sealed class SftpConnectionOptions(
    string host,
    int port,
    string username,
    string password,
    bool acceptAnySshHostKey = false)
{
    /// <summary>
    /// SFTPサーバーのホスト名またはIPアドレス
    /// </summary>
    public string Host { get; } = host ?? throw new ArgumentNullException(nameof(host));

    /// <summary>
    /// SFTPサーバーのポート番号（通常は22）
    /// SSHプロトコルの標準ポートを使用
    /// </summary>
    public int Port { get; } = port;

    /// <summary>
    /// SSH認証に使用するユーザー名
    /// SFTPはSSHをベースにしたプロトコルなのでSSHユーザーが必要
    /// </summary>
    public string Username { get; } = username ?? throw new ArgumentNullException(nameof(username));

    /// <summary>
    /// SSH認証に使用するパスワード
    /// SSHはパスワード認証の他に公開鍵認証もサポートしている
    /// セキュリティ上、本番環境では公開鍵認証の使用を推奨
    /// </summary>
    public string Password { get; } = password ?? throw new ArgumentNullException(nameof(password));

    /// <summary>
    /// SSHホストキーの検証を無効にするかどうか
    /// trueの場合、任意のホストキーを受け入れる（テスト環境専用）
    /// 本番環境では必ずfalseにしてknown_hostsファイルによる適切なホストキー検証を行うべき
    /// </summary>
    public bool AcceptAnySshHostKey { get; } = acceptAnySshHostKey;
}