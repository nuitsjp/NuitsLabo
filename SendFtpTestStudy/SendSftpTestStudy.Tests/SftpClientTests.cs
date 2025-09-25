using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SendSftpTestStudy.Tests.Infrastructure;
using Shouldly;

namespace SendSftpTestStudy.Tests;

/// <summary>
/// SftpClientの振る舞いを検証するテストクラス
/// FxSshで起動したテスト用SFTPサーバーと実際に通信し、アップロード機能を確認する
/// </summary>
/// <param name="fixture">SFTPサーバーのテストフィクスチャ</param>
public class SftpClientTests(SftpServerFixture fixture) : IClassFixture<SftpServerFixture>
{
    [Fact]
    public async Task UploadOverSftp()
    {
        // ------------------------------------------------------------------------------------
        // Arrange
        // ------------------------------------------------------------------------------------

        var builder = Host.CreateApplicationBuilder();
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

        builder.Configuration["SftpClient:Port"] = fixture.Port.ToString();

        builder.Services
            .AddScoped<ISftpClientProvider, SftpClientProvider>()
            .AddOptions<SftpConnectionOptions>().BindConfiguration("SftpClient");

        await using var serviceProvider = builder.Services.BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<ISftpClientProvider>();

        var remotePath = "/data/inbound/world.txt";
        var payload = "Hello via SFTP";

        // ------------------------------------------------------------------------------------
        // Act
        // ------------------------------------------------------------------------------------

        await using var client = await provider.CreateAsync();
        await using (var uploadStream = new MemoryStream(Encoding.UTF8.GetBytes(payload)))
        {
            await client.UploadAsync(remotePath, uploadStream);
        }

        // ------------------------------------------------------------------------------------
        // Assert
        // ------------------------------------------------------------------------------------

        var expectedLocalPath = Path.Combine(fixture.RootPath, "data", "inbound", "world.txt");
        File.Exists(expectedLocalPath).ShouldBeTrue();

        var content = await File.ReadAllTextAsync(expectedLocalPath);
        content.ShouldBe(payload);
    }
}
