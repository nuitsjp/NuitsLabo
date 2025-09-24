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

        // アップロード先のリモートパス（ディレクトリは事前に存在している必要がある）
        var remotePath = "/uploads/hello.txt";

        // アップロードするテストデータ
        var payload = "Hello via FTP";

        // FtpClientProviderを使用してFtpClientを作成し、アップロード実行
        await using var client = await FtpClientProvider.CreateAsync(options);
        await using (var uploadStream = new MemoryStream(Encoding.UTF8.GetBytes(payload)))
        {
            await client.UploadAsync(remotePath, uploadStream);
        }

        // サーバーサイドのローカルファイルシステムでファイルの存在を検証
        var expectedLocalPath = Path.Combine(fixture.RootPath, "uploads", "hello.txt");
        Assert.True(File.Exists(expectedLocalPath));

        // アップロードされたファイルの内容が期待値と一致するか検証
        Assert.Equal(payload, await File.ReadAllTextAsync(expectedLocalPath));
    }
}