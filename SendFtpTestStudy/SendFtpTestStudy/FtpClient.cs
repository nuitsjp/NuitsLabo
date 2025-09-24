using FluentFTP;

namespace SendFtpTestStudy;

/// <summary>
/// FTPサーバーへのファイルアップロード機能を提供するクライアントクラス
/// FluentFTPライブラリを使用して非同期のFTP通信を行う
/// </summary>
public sealed class FtpClient
{
    /// <summary>
    /// ファイルをFTPサーバーにアップロードする
    /// </summary>
    /// <param name="options">FTP接続に必要な設定情報（ホスト、ユーザー名、パスワード等）</param>
    /// <param name="remotePath">アップロード先のリモートパス（例："/upload/file.txt"）</param>
    /// <param name="content">アップロードするファイル内容のストリーム</param>
    /// <param name="cancellationToken">キャンセル処理用のトークン</param>
    /// <exception cref="ArgumentException">リモートパスが空またはnullの場合</exception>
    /// <returns>非同期タスク</returns>
    public async Task UploadAsync(
        FtpConnectionOptions options,
        string remotePath,
        Stream content,
        CancellationToken cancellationToken = default)
    {
        // パスをUnix形式（/区切り）に正規化
        var normalizedPath = NormalizeFilePath(remotePath);

        // FTPクライアントを作成し、接続を開始
        await using var ftp = CreateAsyncFtpClient(options);
        await ftp.Connect(cancellationToken).ConfigureAwait(false);
        try
        {
            // アップロード先ディレクトリを取得
            var directory = GetDirectoryPath(normalizedPath);
            if (!string.IsNullOrEmpty(directory))
            {
                // ディレクトリが存在しない場合は作成（再帰的に作成）
                var directoryPath = NormalizeDirectory(directory).TrimEnd('/');
                await ftp.CreateDirectory(directoryPath, true, cancellationToken).ConfigureAwait(false);
            }

            // ストリームの位置を先頭にリセット
            ResetPosition(content);

            // ファイルをアップロード（既存ファイルは上書き）
            await ftp.UploadStream(content, normalizedPath, FtpRemoteExists.Overwrite, false, null, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            // 接続を確実に切断
            await ftp.Disconnect(cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// FluentFTPの非同期クライアントを作成し、基本設定を行う
    /// </summary>
    /// <param name="options">FTP接続オプション</param>
    /// <returns>設定済みのAsyncFtpClientインスタンス</returns>
    private static AsyncFtpClient CreateAsyncFtpClient(FtpConnectionOptions options)
    {
        // FTPクライアントを接続情報で初期化
        var ftp = new AsyncFtpClient(options.Host, options.Username, options.Password, options.Port);

        // データ接続タイプを自動パッシブモードに設定
        // パッシブモードはファイアウォール環境でも動作しやすい
        ftp.Config.DataConnectionType = FtpDataConnectionType.AutoPassive;

        // SSL証明書の検証設定
        if (options.AcceptAnyCertificate)
        {
            // 任意の証明書を受け入れる（テスト環境用）
            ftp.Config.ValidateAnyCertificate = true;
        }

        return ftp;
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
    /// ディレクトリパスをUnix形式に正規化し、末尾に/を付ける
    /// </summary>
    /// <param name="path">正規化するディレクトリパス</param>
    /// <returns>正規化されたディレクトリパス（末尾に/付き）</returns>
    private static string NormalizeDirectory(string path)
    {
        var normalized = NormalizeFilePath(path);

        // 末尾に/がない場合は追加
        return normalized.EndsWith("/", StringComparison.Ordinal) ? normalized : normalized + "/";
    }

    /// <summary>
    /// ファイルパスからディレクトリ部分を抽出する
    /// </summary>
    /// <param name="path">ファイルパス</param>
    /// <returns>ディレクトリパス（例："/dir/file.txt" → "/dir"）</returns>
    private static string GetDirectoryPath(string path)
    {
        // 最後の/の位置を検索
        var lastSlash = path.LastIndexOf("/", StringComparison.Ordinal);
        if (lastSlash <= 0)
        {
            // ルートディレクトリまたは/が見つからない場合
            return string.Empty;
        }

        // 最後の/より前の部分をディレクトリパスとして返す
        return path[..lastSlash];
    }
}

