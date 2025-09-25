namespace SendSftpTestStudy;

/// <summary>
/// SFTP接続に必要な設定値を保持するためのオプションモデル
/// Microsoft.Extensions.Configurationからのバインドを想定してプロパティを書き換え可能にしている
/// </summary>
public sealed class SftpConnectionOptions
{
    /// <summary>
    /// オプションのデフォルト値を初期化するパラメーターなしコンストラクター
    /// </summary>
    public SftpConnectionOptions()
    {
        Host = string.Empty;
        Username = string.Empty;
        Password = string.Empty;
        Port = 22;
        AcceptAnySshHostKey = false;
    }

    /// <summary>
    /// 全プロパティを指定して初期化するコンストラクター
    /// </summary>
    public SftpConnectionOptions(
        string host,
        int port,
        string username,
        string password,
        bool acceptAnySshHostKey = false)
        : this()
    {
        Host = host ?? throw new ArgumentNullException(nameof(host));
        Port = port;
        Username = username ?? throw new ArgumentNullException(nameof(username));
        Password = password ?? throw new ArgumentNullException(nameof(password));
        AcceptAnySshHostKey = acceptAnySshHostKey;
    }

    /// <summary>
    /// SFTPサーバーのホスト名またはIPアドレス
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// SFTPサーバーのポート番号（既定は22）
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// SSH認証に使用するユーザー名
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// SSH認証に使用するパスワード
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 任意のSSHホストキーを受け入れるかどうか
    /// テスト環境ではtrue、本番環境ではknown_hostsの検証を推奨
    /// </summary>
    public bool AcceptAnySshHostKey { get; set; }
}
