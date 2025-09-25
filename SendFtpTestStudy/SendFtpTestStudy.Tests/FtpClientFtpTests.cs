using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SendFtpTestStudy.Tests.Infrastructure;

namespace SendFtpTestStudy.Tests;

/// <summary>
/// FtpClientクラスの統合テストクラス
/// テスト用のFTPサーバーを使用して、実際のFTP通信を行いアップロード機能を検証する
/// xUnitのIClassFixtureを使用してテストクラス全体でFTPサーバーを共有
/// ServiceCollectionを使用してDI経由でFtpClientProviderを取得する
/// </summary>
/// <param name="fixture">FTPサーバーのテストフィクスチャ</param>
public class FtpClientFtpTests(FtpServerFixture fixture) : IClassFixture<FtpServerFixture>
{

    /// <summary>
    /// FTPサーバーへのファイルアップロード機能をテストする
    /// テストシナリオ：
    /// 1. appsettings.jsonから設定を読み込み
    /// 2. 動的ポートで設定を上書き
    /// 3. ServiceCollectionにFtpClientProviderを登録
    /// 4. DIコンテナからIFtpClientProviderを取得
    /// 5. メモリストリームでテキストデータを作成
    /// 6. FtpClient.UploadAsyncでファイルをアップロード
    /// 7. サーバーのローカルファイルシステムでファイルの存在と内容を検証
    /// </summary>
    [Fact]
    public async Task UploadOverFtp()
    {
        // appsettings.jsonから設定を読み込み
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        // テスト用FTPサーバーの動的ポートで設定を上書き
        configuration["FtpConnection:Port"] = fixture.Port.ToString();

        // ServiceCollectionを設定してDI経由でFtpClientProviderを取得
        var services = new ServiceCollection();
        services.AddFtpClient(configuration);

        await using var serviceProvider = services.BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<IFtpClientProvider>();

        // アップロード先のリモートパス（ディレクトリは事前に存在している必要がある）
        var remotePath = "/uploads/hello.txt";

        // アップロードするテストデータ
        var payload = "Hello via FTP";

        // DI経由でFtpClientを作成し、アップロード実行
        await using var client = await provider.CreateAsync();
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