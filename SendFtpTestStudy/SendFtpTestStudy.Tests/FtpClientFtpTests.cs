using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
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

        builder.Services.AddFtpClientProvider(builder.Configuration, "FtpClient");

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

    [Fact]
    public async Task FtpClientProvider_CreateAsync_WhenHostMissing_Throws()
    {
        // ------------------------------------------------------------------------------------
        // Arrange
        // ------------------------------------------------------------------------------------

        var builder = Host.CreateApplicationBuilder();
        
        // Host を空文字列に設定（無効な値）
        builder.Configuration["FtpClient:Host"] = "";
        builder.Configuration["FtpClient:Port"] = "21";
        builder.Configuration["FtpClient:User"] = "tester";
        builder.Configuration["FtpClient:Password"] = "test-pass";

        builder.Services.AddFtpClientProvider(builder.Configuration, "FtpClient");

        await using var serviceProvider = builder.Services.BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<IFtpClientProvider>();

        // ------------------------------------------------------------------------------------
        // Act & Assert
        // ------------------------------------------------------------------------------------

        var exception = await Should.ThrowAsync<OptionsValidationException>(() => provider.CreateAsync());
        exception.Message.ShouldContain("Host");
    }

    [Fact]
    public async Task FtpClientProvider_CreateAsync_WhenPortInvalid_Throws()
    {
        // ------------------------------------------------------------------------------------
        // Arrange
        // ------------------------------------------------------------------------------------

        var builder = Host.CreateApplicationBuilder();
        
        builder.Configuration["FtpClient:Host"] = "127.0.0.1";
        builder.Configuration["FtpClient:Port"] = "70000"; // 無効なポート番号
        builder.Configuration["FtpClient:User"] = "tester";
        builder.Configuration["FtpClient:Password"] = "test-pass";

        builder.Services.AddFtpClientProvider(builder.Configuration, "FtpClient");

        await using var serviceProvider = builder.Services.BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<IFtpClientProvider>();

        // ------------------------------------------------------------------------------------
        // Act & Assert
        // ------------------------------------------------------------------------------------

        var exception = await Should.ThrowAsync<OptionsValidationException>(() => provider.CreateAsync());
        exception.Message.ShouldContain("Port");
    }

    [Fact]
    public async Task FtpClientProvider_CreateAsync_WhenUserMissing_Throws()
    {
        // ------------------------------------------------------------------------------------
        // Arrange
        // ------------------------------------------------------------------------------------

        var builder = Host.CreateApplicationBuilder();
        
        builder.Configuration["FtpClient:Host"] = "127.0.0.1";
        builder.Configuration["FtpClient:Port"] = "21";
        builder.Configuration["FtpClient:User"] = ""; // 空のユーザー名
        builder.Configuration["FtpClient:Password"] = "test-pass";

        builder.Services.AddFtpClientProvider(builder.Configuration, "FtpClient");

        await using var serviceProvider = builder.Services.BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<IFtpClientProvider>();

        // ------------------------------------------------------------------------------------
        // Act & Assert
        // ------------------------------------------------------------------------------------

        var exception = await Should.ThrowAsync<OptionsValidationException>(() => provider.CreateAsync());
        exception.Message.ShouldContain("User");
    }

    [Fact]
    public async Task FtpClientProvider_CreateAsync_WhenPasswordMissing_Throws()
    {
        // ------------------------------------------------------------------------------------
        // Arrange
        // ------------------------------------------------------------------------------------

        var builder = Host.CreateApplicationBuilder();
        
        builder.Configuration["FtpClient:Host"] = "127.0.0.1";
        builder.Configuration["FtpClient:Port"] = "21";
        builder.Configuration["FtpClient:User"] = "tester";
        builder.Configuration["FtpClient:Password"] = ""; // 空のパスワード

        builder.Services.AddFtpClientProvider(builder.Configuration, "FtpClient");

        await using var serviceProvider = builder.Services.BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<IFtpClientProvider>();

        // ------------------------------------------------------------------------------------
        // Act & Assert
        // ------------------------------------------------------------------------------------

        var exception = await Should.ThrowAsync<OptionsValidationException>(() => provider.CreateAsync());
        exception.Message.ShouldContain("Password");
    }

    [Fact]
    public async Task FtpClientProvider_ValidConfiguration_IntegrationTest()
    {
        // ------------------------------------------------------------------------------------
        // Arrange
        // ------------------------------------------------------------------------------------

        var builder = Host.CreateApplicationBuilder();
        
        // 有効な設定を使用（バリデーションをパスする）
        builder.Configuration["FtpClient:Host"] = "127.0.0.1";
        builder.Configuration["FtpClient:Port"] = ftpServer.Port.ToString();
        builder.Configuration["FtpClient:User"] = "tester";
        builder.Configuration["FtpClient:Password"] = "test-pass";
        builder.Configuration["FtpClient:DataConnectionType"] = "AutoPassive";

        builder.Services.AddFtpClientProvider(builder.Configuration, "FtpClient");

        await using var serviceProvider = builder.Services.BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<IFtpClientProvider>();

        // ------------------------------------------------------------------------------------
        // Act & Assert
        // ------------------------------------------------------------------------------------

        // バリデーションが成功し、FtpClientが正常に作成されることを確認
        await using var client = await provider.CreateAsync();
        client.ShouldNotBeNull();
    }

    [Fact]
    public async Task FtpClientProvider_CreateAsync_WhenRetryCountInvalid_Throws()
    {
        // ------------------------------------------------------------------------------------
        // Arrange
        // ------------------------------------------------------------------------------------

        var builder = Host.CreateApplicationBuilder();
        
        builder.Configuration["FtpClient:Host"] = "127.0.0.1";
        builder.Configuration["FtpClient:Port"] = "21";
        builder.Configuration["FtpClient:User"] = "tester";
        builder.Configuration["FtpClient:Password"] = "test-pass";
        builder.Configuration["FtpClient:RetryCount"] = "-1"; // 無効な再試行回数

        builder.Services.AddFtpClientProvider(builder.Configuration, "FtpClient");

        await using var serviceProvider = builder.Services.BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<IFtpClientProvider>();

        // ------------------------------------------------------------------------------------
        // Act & Assert
        // ------------------------------------------------------------------------------------

        var exception = await Should.ThrowAsync<OptionsValidationException>(() => provider.CreateAsync());
        exception.Message.ShouldContain("RetryCount");
    }

    [Fact]
    public async Task FtpClientProvider_CreateAsync_WhenRetryIntervalInvalid_Throws()
    {
        // ------------------------------------------------------------------------------------
        // Arrange
        // ------------------------------------------------------------------------------------

        var builder = Host.CreateApplicationBuilder();
        
        builder.Configuration["FtpClient:Host"] = "127.0.0.1";
        builder.Configuration["FtpClient:Port"] = "21";
        builder.Configuration["FtpClient:User"] = "tester";
        builder.Configuration["FtpClient:Password"] = "test-pass";
        builder.Configuration["FtpClient:RetryInterval"] = "0"; // 無効な再試行間隔

        builder.Services.AddFtpClientProvider(builder.Configuration, "FtpClient");

        await using var serviceProvider = builder.Services.BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<IFtpClientProvider>();

        // ------------------------------------------------------------------------------------
        // Act & Assert
        // ------------------------------------------------------------------------------------

        var exception = await Should.ThrowAsync<OptionsValidationException>(() => provider.CreateAsync());
        exception.Message.ShouldContain("RetryInterval");
    }

    [Fact]
    public async Task FtpClientProvider_CreateAsync_WhenConnectionTimeoutInvalid_Throws()
    {
        // ------------------------------------------------------------------------------------
        // Arrange
        // ------------------------------------------------------------------------------------

        var builder = Host.CreateApplicationBuilder();
        
        builder.Configuration["FtpClient:Host"] = "127.0.0.1";
        builder.Configuration["FtpClient:Port"] = "21";
        builder.Configuration["FtpClient:User"] = "tester";
        builder.Configuration["FtpClient:Password"] = "test-pass";
        builder.Configuration["FtpClient:ConnectionTimeout"] = "500"; // 無効な接続タイムアウト

        builder.Services.AddFtpClientProvider(builder.Configuration, "FtpClient");

        await using var serviceProvider = builder.Services.BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<IFtpClientProvider>();

        // ------------------------------------------------------------------------------------
        // Act & Assert
        // ------------------------------------------------------------------------------------

        var exception = await Should.ThrowAsync<OptionsValidationException>(() => provider.CreateAsync());
        exception.Message.ShouldContain("ConnectionTimeout");
    }

    [Fact]
    public async Task FtpClientProvider_CreateAsync_WhenDataTimeoutInvalid_Throws()
    {
        // ------------------------------------------------------------------------------------
        // Arrange
        // ------------------------------------------------------------------------------------

        var builder = Host.CreateApplicationBuilder();
        
        builder.Configuration["FtpClient:Host"] = "127.0.0.1";
        builder.Configuration["FtpClient:Port"] = "21";
        builder.Configuration["FtpClient:User"] = "tester";
        builder.Configuration["FtpClient:Password"] = "test-pass";
        builder.Configuration["FtpClient:DataTimeout"] = "500"; // 無効なデータタイムアウト

        builder.Services.AddFtpClientProvider(builder.Configuration, "FtpClient");

        await using var serviceProvider = builder.Services.BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<IFtpClientProvider>();

        // ------------------------------------------------------------------------------------
        // Act & Assert
        // ------------------------------------------------------------------------------------

        var exception = await Should.ThrowAsync<OptionsValidationException>(() => provider.CreateAsync());
        exception.Message.ShouldContain("DataTimeout");
    }

    [Fact]
    public async Task CreateAsync_RetriesOnTransientFailure()
    {
        // ------------------------------------------------------------------------------------
        // Arrange
        // ------------------------------------------------------------------------------------

        var builder = Host.CreateApplicationBuilder();
        
        // 間違ったポートを指定してConnectionRefusedエラーをシミュレート
        builder.Configuration["FtpClient:Host"] = "127.0.0.1";
        builder.Configuration["FtpClient:Port"] = "22222"; // 使用されていないポート
        builder.Configuration["FtpClient:User"] = "tester";
        builder.Configuration["FtpClient:Password"] = "test-pass";
        builder.Configuration["FtpClient:RetryCount"] = "2";
        builder.Configuration["FtpClient:RetryInterval"] = "500"; // 短い間隔でテスト高速化
        builder.Configuration["FtpClient:ConnectionTimeout"] = "2000"; // 短いタイムアウト

        builder.Services.AddFtpClientProvider(builder.Configuration, "FtpClient");

        await using var serviceProvider = builder.Services.BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<IFtpClientProvider>();

        // ------------------------------------------------------------------------------------
        // Act & Assert
        // ------------------------------------------------------------------------------------

        // 再試行後も接続に失敗することを確認
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        await Should.ThrowAsync<Exception>(() => provider.CreateAsync());
        stopwatch.Stop();

        // 再試行により実行時間が延びることを確認（概算でチェック）
        // 再試行間隔 500ms × 2回 = 1000ms以上の追加時間
        stopwatch.ElapsedMilliseconds.ShouldBeGreaterThan(800);
    }

    [Fact]
    public async Task CreateAsync_TimesOutOnSlowConnection()
    {
        // ------------------------------------------------------------------------------------
        // Arrange
        // ------------------------------------------------------------------------------------

        var builder = Host.CreateApplicationBuilder();
        
        // ルーティングされないアドレスでタイムアウトをシミュレート
        // 10.255.255.1は予約されたプライベートアドレスで応答なし
        builder.Configuration["FtpClient:Host"] = "10.255.255.1";
        builder.Configuration["FtpClient:Port"] = "21";
        builder.Configuration["FtpClient:User"] = "tester";
        builder.Configuration["FtpClient:Password"] = "test-pass";
        builder.Configuration["FtpClient:RetryCount"] = "0"; // リトライなし
        builder.Configuration["FtpClient:ConnectionTimeout"] = "2000"; // 短いタイムアウト

        builder.Services.AddFtpClientProvider(builder.Configuration, "FtpClient");

        await using var serviceProvider = builder.Services.BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<IFtpClientProvider>();

        // ------------------------------------------------------------------------------------
        // Act & Assert
        // ------------------------------------------------------------------------------------

        // タイムアウトにより例外が発生することを確認
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        await Should.ThrowAsync<Exception>(() => provider.CreateAsync());
        stopwatch.Stop();

        // タイムアウト設定に近い時間で処理が終了することを確認（余裕をもって4秒以内）
        stopwatch.ElapsedMilliseconds.ShouldBeLessThan(4000);
        
        // 少なくともタイムアウト設定の時間は経過していることを確認
        stopwatch.ElapsedMilliseconds.ShouldBeGreaterThan(1500);
    }
}