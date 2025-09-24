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
    /// 送信先ディレクトリは事前に存在している必要があり、存在しない場合は例外が発生する
    /// リモートパスはUNIX形式（/区切り）で渡される前提とする
    /// </summary>
    /// <param name="options">FTP接続に必要な設定情報（ホスト、ユーザー名、パスワード等）</param>
    /// <param name="remotePath">アップロード先のリモートパス（UNIX形式、例："/upload/file.txt"）</param>
    /// <param name="content">アップロードするファイル内容のストリーム</param>
    /// <param name="cancellationToken">キャンセル処理用のトークン</param>
    /// <exception cref="ArgumentException">リモートパスが空またはnullの場合</exception>
    /// <exception cref="FluentFTP.Exceptions.FtpException">送信先ディレクトリが存在しない場合</exception>
    /// <returns>非同期タスク</returns>
    public async Task UploadAsync(
        FtpConnectionOptions options,
        string remotePath,
        Stream content,
        CancellationToken cancellationToken = default)
    {
        // FTPクライアントを作成し、接続を開始
        await using var ftp = CreateAsyncFtpClient(options);
        await ftp.Connect(cancellationToken).ConfigureAwait(false);
        try
        {
            // ファイルをアップロード（既存ファイルは上書き）
            await ftp.UploadStream(content, remotePath, FtpRemoteExists.Overwrite, false, null, cancellationToken).ConfigureAwait(false);
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

        // データ接続タイプを設定
        ftp.Config.DataConnectionType = options.DataConnectionType;

        // SSL証明書の検証設定
        if (options.AcceptAnyCertificate)
        {
            // 任意の証明書を受け入れる（テスト環境用）
            ftp.Config.ValidateAnyCertificate = true;
        }

        return ftp;
    }




}

