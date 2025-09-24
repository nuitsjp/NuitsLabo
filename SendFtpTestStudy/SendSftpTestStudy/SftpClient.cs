using System.Linq;
using Renci.SshNet;

namespace SendSftpTestStudy;

/// <summary>
/// SFTPサーバーへのファイルアップロード機能を提供するクライアントクラス
/// SSH.NETライブラリを使用してSSH接続経由でのファイル転送を行う
/// </summary>
public sealed class SftpClient
{
    /// <summary>
    /// ファイルをSFTPサーバーにアップロードする
    /// </summary>
    /// <param name="options">SFTP接続に必要な設定情報（ホスト、ユーザー名、パスワード等）</param>
    /// <param name="remotePath">アップロード先のリモートパス（例："/upload/file.txt"）</param>
    /// <param name="content">アップロードするファイル内容のストリーム</param>
    /// <param name="cancellationToken">キャンセル処理用のトークン</param>
    /// <exception cref="ArgumentException">リモートパスが空またはnullの場合</exception>
    /// <returns>非同期タスク</returns>
    public async Task UploadAsync(
        SftpConnectionOptions options,
        string remotePath,
        Stream content,
        CancellationToken cancellationToken = default)
    {
        // リモートパスの妥当性チェック
        if (string.IsNullOrWhiteSpace(remotePath))
        {
            throw new ArgumentException("Remote path is required.", nameof(remotePath));
        }

        // キャンセル要求がある場合は処理を中止
        cancellationToken.ThrowIfCancellationRequested();

        // SFTPクライアントを作成し、接続を開始
        using var client = CreateSftpClient(options);
        client.Connect();

        try
        {
            // パスをUnix形式（/区切り）に正規化
            var normalizedPath = NormalizeFilePath(remotePath);

            // アップロード先ディレクトリを取得
            var directory = GetDirectoryPath(normalizedPath);

            // ディレクトリが存在しない場合は再帰的に作成
            EnsureDirectoryExists(client, directory);

            // ストリームの位置を先頭にリセット
            ResetPosition(content);

            // ファイルをアップロード（Task.Runで非同期化、進行状況コールバックでキャンセル監視）
            await Task.Run(
                () => client.UploadFile(content, normalizedPath, true, _ => cancellationToken.ThrowIfCancellationRequested()),
                cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            // 接続が確立されている場合は切断
            if (client.IsConnected)
            {
                client.Disconnect();
            }
        }
    }

    /// <summary>
    /// SSH.NETのSFTPクライアントを作成し、基本設定を行う
    /// </summary>
    /// <param name="options">SFTP接続オプション</param>
    /// <returns>設定済みのSftpClientインスタンス</returns>
    private static Renci.SshNet.SftpClient CreateSftpClient(SftpConnectionOptions options)
    {
        // SFTPクライアントを接続情報で初期化
        var client = new Renci.SshNet.SftpClient(options.Host, options.Port, options.Username, options.Password);

        // SSHホストキーの検証設定
        if (options.AcceptAnySshHostKey)
        {
            // 任意のホストキーを受け入れる（テスト環境用）
            // 本番環境では既知のホストキーのみを受け入れるべき
            client.HostKeyReceived += (_, e) => e.CanTrust = true;
        }

        return client;
    }

    /// <summary>
    /// ストリームの位置を先頭にリセットする
    /// アップロード前にストリームを最初から読み取れるようにする
    /// </summary>
    /// <param name="stream">位置をリセットするストリーム</param>
    private static void ResetPosition(Stream stream)
    {
        // シーク可能なストリームの場合のみ位置をリセット
        if (stream.CanSeek)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }
    }

    /// <summary>
    /// SFTPサーバー上に指定されたディレクトリパスが存在することを保証する
    /// 存在しないディレクトリは再帰的に作成される
    /// </summary>
    /// <param name="client">接続済みのSFTPクライアント</param>
    /// <param name="remoteDirectory">作成するディレクトリパス</param>
    private static void EnsureDirectoryExists(Renci.SshNet.SftpClient client, string remoteDirectory)
    {
        // ディレクトリパスをセグメント（各ディレクトリ名）に分割
        var segments = SplitSegments(remoteDirectory);
        if (!segments.Any())
        {
            // セグメントがない場合は処理不要
            return;
        }

        // ルートから順次ディレクトリを作成
        var current = "/";
        foreach (var segment in segments)
        {
            // パスを構築（ルートの場合の特別処理）
            current = current == "/" ? $"/{segment}" : $"{current}/{segment}";

            // ディレクトリが存在しない場合は作成
            if (!client.Exists(current))
            {
                client.CreateDirectory(current);
            }
        }
    }

    /// <summary>
    /// ファイルパスをUnix形式に正規化する
    /// Windowsの\区切りを/区切りに変換し、先頭に/を付ける
    /// </summary>
    /// <param name="path">正規化するパス</param>
    /// <returns>正規化されたパス（例："\dir\file.txt" → "/dir/file.txt"）</returns>
    private static string NormalizeFilePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return "/";
        }

        // Windowsの\をUnixの/に変換
        var normalized = path.Replace("\\", "/");

        // 先頭に/がない場合は追加
        return normalized.StartsWith("/", StringComparison.Ordinal) ? normalized : "/" + normalized;
    }

    /// <summary>
    /// ファイルパスからディレクトリ部分を抽出する
    /// </summary>
    /// <param name="path">ファイルパス</param>
    /// <returns>ディレクトリパス（例："/dir/file.txt" → "/dir"）</returns>
    private static string GetDirectoryPath(string path)
    {
        var normalized = NormalizeFilePath(path);

        // 最後の/の位置を検索
        var lastSlash = normalized.LastIndexOf("/", StringComparison.Ordinal);
        if (lastSlash <= 0)
        {
            // ルートディレクトリまたは/が見つからない場合
            return string.Empty;
        }

        // 最後の/より前の部分をディレクトリパスとして返す
        return normalized[..lastSlash];
    }

    /// <summary>
    /// パス文字列をディレクトリセグメントに分割する
    /// 空のセグメントや空白のみのセグメントは除外される
    /// </summary>
    /// <param name="path">分割するパス文字列</param>
    /// <returns>ディレクトリ名のコレクション（例："/a/b/c" → ["a", "b", "c"]）</returns>
    private static IEnumerable<string> SplitSegments(string path)
    {
        return path
            .Replace("\\", "/")  // Windowsパスを正規化
            .Split("/", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
}

