using FluentFTP;

namespace SendFtpTestStudy;

/// <summary>
/// FTPサーバーへのファイルアップロード機能を提供するクライアントクラス
/// FluentFTPライブラリを使用して非同期のFTP通信を行う
/// </summary>
public sealed class FtpClient : IFtpClient
{
    private readonly IAsyncFtpClient _ftp;

    /// <summary>
    /// FtpClientのコンストラクタ（内部使用）
    /// 接続済みのAsyncFtpClientを受け取る
    /// </summary>
    /// <param name="asyncFtpClient">接続済みのAsyncFtpClientインスタンス</param>
    internal FtpClient(IAsyncFtpClient asyncFtpClient)
    {
        _ftp = asyncFtpClient;
    }

    /// <summary>
    /// ファイルをFTPサーバーにアップロードする
    /// 送信先ディレクトリは事前に存在している必要があり、存在しない場合は例外が発生する
    /// リモートパスはUNIX形式（/区切り）で渡される前提とする
    /// </summary>
    /// <param name="remotePath">アップロード先のリモートパス（UNIX形式、例："/upload/file.txt"）</param>
    /// <param name="content">アップロードするファイル内容のストリーム</param>
    /// <param name="cancellationToken">キャンセル処理用のトークン</param>
    public async Task UploadAsync(
        string remotePath,
        Stream content,
        CancellationToken cancellationToken = default)
    {
        // ファイルをアップロード（既存ファイルは上書き）
        await _ftp.UploadStream(content, remotePath, FtpRemoteExists.Overwrite, false, null, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// FTPクライアントのリソースを非同期で解放する
    /// </summary>
    /// <returns>解放処理を表すValueTask</returns>
    public async ValueTask DisposeAsync()
    {
        if (_ftp.IsConnected)
        {
            await _ftp.Disconnect(CancellationToken.None).ConfigureAwait(false);
        }
        await _ftp.DisposeAsync().ConfigureAwait(false);
    }
}

