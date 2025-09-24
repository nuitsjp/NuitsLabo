using System.Text;
using SendFtpTestStudy.Tests.Infrastructure;

namespace SendFtpTestStudy.Tests;

/// <summary>
/// FtpClientクラスの統合テストクラス
/// テスト用のFTPサーバーを使用して、実際のFTP通信を行いアップロード機能を検証する
/// xUnitのIClassFixtureを使用してテストクラス全体でFTPサーバーを共有
/// </summary>
/// <param name="fixture">FTPサーバーのテストフィクスチャ</param>
public class FtpClientFtpTests(FtpServerFixture fixture) : IClassFixture<FtpServerFixture>
{
    /// <summary>
    /// テスト対象のFtpClientインスタンス。
    /// 各テストメソッドで共有され、テスト用FTPサーバーへのアップロード機能をテストする。
    /// </summary>
    private readonly FtpClient _client = new();

    /// <summary>
    /// FTPサーバーへのファイルアップロード機能をテストする
    /// テストシナリオ：
    /// 1. メモリストリームでテキストデータを作成
    /// 2. FtpClient.UploadAsyncでファイルをアップロード
    /// 3. サーバーのローカルファイルシステムでファイルの存在と内容を検証
    /// </summary>
    [Fact]
    public async Task UploadOverFtp()
    {
        // テスト用FTPサーバーの接続オプションを取得
        var options = fixture.Options;

        // アップロード先のリモートパス（ディレクトリは自動作成される）
        var remotePath = "/uploads/hello.txt";

        // アップロードするテストデータ
        var payload = "Hello via FTP";

        // メモリストリームでテストデータをラップしてアップロード実行
        await using (var uploadStream = new MemoryStream(Encoding.UTF8.GetBytes(payload)))
        {
            await _client.UploadAsync(options, remotePath, uploadStream);
        }

        // サーバーサイドのローカルファイルシステムでファイルの存在を検証
        var expectedLocalPath = Path.Combine(fixture.RootPath, "uploads", "hello.txt");
        Assert.True(File.Exists(expectedLocalPath));

        // アップロードされたファイルの内容が期待値と一致するか検証
        Assert.Equal(payload, await File.ReadAllTextAsync(expectedLocalPath));
    }
}