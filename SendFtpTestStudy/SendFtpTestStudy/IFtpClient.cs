namespace SendFtpTestStudy;

/// <summary>
/// FTPサーバーへのファイルアップロード機能を提供するインターフェイス
/// 非同期FTP通信のメソッドを定義する
/// </summary>
public interface IFtpClient : IAsyncDisposable
{
    /// <summary>
    /// ファイルをFTPサーバーにアップロードする
    /// 送信先ディレクトリは事前に存在している必要があり、存在しない場合は例外が発生する
    /// リモートパスはUNIX形式（/区切り）で渡される前提とする
    /// </summary>
    /// <param name="remotePath">アップロード先のリモートパス（UNIX形式、例："/upload/file.txt"）</param>
    /// <param name="content">アップロードするファイル内容のストリーム</param>
    /// <param name="cancellationToken">キャンセル処理用のトークン</param>
    Task UploadAsync(
        string remotePath,
        Stream content,
        CancellationToken cancellationToken = default);
}