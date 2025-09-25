using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SendFtpTestStudy.Tests.Infrastructure;
using Shouldly;

namespace SendFtpTestStudy.Tests;

/// <summary>
/// FtpClientクラスの統合テストクラス
/// テスト用のFTPサーバーを使用して、実際のFTP通信を行いアップロード機能を検証する
/// xUnitのIClassFixtureを使用してテストクラス全体でFTPサーバーを共有
/// ServiceCollectionを使用してDI経由でFtpClientProviderを取得する
/// </summary>
public class FtpClientFtpTests(FtpServer ftpServer) : IClassFixture<FtpServer>
{

    [Fact]
    public async Task UploadOverFtp()
    {
        // ------------------------------------------------------------------------------------
        // Arrange
        // ------------------------------------------------------------------------------------

        var builder = Host.CreateApplicationBuilder();
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");


        // テスト用FTPサーバーの動的ポートで設定を上書き
        builder.Configuration["FtpClient:Port"] = ftpServer.Port.ToString();

        builder.Services
            .AddTransient<IFtpClientProvider, FtpClientProvider>()
            .AddOptions<FtpConnectionOptions>().BindConfiguration("FtpClient"); 

        await using var serviceProvider = builder.Services.BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<IFtpClientProvider>();

        // アップロード先のリモートパス（ディレクトリは事前に存在している必要がある）
        var remotePath = "/uploads/hello.txt";

        // アップロードするテストデータ
        var payload = "Hello via FTP";

        // ------------------------------------------------------------------------------------
        // Act
        // ------------------------------------------------------------------------------------

        // DI経由でFtpClientを作成し、アップロード実行
        await using var client = await provider.CreateAsync();
        await using (var uploadStream = new MemoryStream(Encoding.UTF8.GetBytes(payload)))
        {
            await client.UploadAsync(remotePath, uploadStream);
        }

        // ------------------------------------------------------------------------------------
        // Assert
        // ------------------------------------------------------------------------------------

        // サーバーサイドのローカルファイルシステムでファイルの存在を検証
        var expectedLocalPath = Path.Combine(ftpServer.RootPath, "uploads", "hello.txt");
        File.Exists(expectedLocalPath).ShouldBeTrue();

        // アップロードされたファイルの内容が期待値と一致するか検証
        var content = await File.ReadAllTextAsync(expectedLocalPath);
        content.ShouldBe(payload);
    }
}