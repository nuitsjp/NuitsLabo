using System.Text;
using SendFtpTestStudy.Tests.Infrastructure;

namespace SendFtpTestStudy.Tests;

public class FtpClientFtpTests(FtpServerFixture fixture) : IClassFixture<FtpServerFixture>
{
    private readonly FtpClient _client = new();

    [Fact]
    public async Task UploadDownloadAndListOverFtp()
    {
        var options = fixture.Options;
        var remotePath = "/uploads/hello.txt";
        var payload = "Hello via FTP";

        await using (var uploadStream = new MemoryStream(Encoding.UTF8.GetBytes(payload)))
        {
            await _client.UploadAsync(options, remotePath, uploadStream);
        }

        var expectedLocalPath = Path.Combine(fixture.RootPath, "uploads", "hello.txt");
        Assert.True(File.Exists(expectedLocalPath));
        Assert.Equal(payload, await File.ReadAllTextAsync(expectedLocalPath));

        var downloaded = await _client.DownloadAsync(options, remotePath);
        Assert.Equal(payload, Encoding.UTF8.GetString(downloaded));

        var entries = await _client.ListAsync(options, "/uploads");
        Assert.Contains("hello.txt", entries);
    }
}

public class FtpClientSftpTests(SftpServerFixture fixture) : IClassFixture<SftpServerFixture>
{
    private readonly FtpClient _client = new();

    [Fact]
    public async Task UploadDownloadAndListOverSftp()
    {
        var options = fixture.Options;
        var remotePath = "/data/inbound/world.txt";
        var payload = "Hello via SFTP";

        await using (var uploadStream = new MemoryStream(Encoding.UTF8.GetBytes(payload)))
        {
            await _client.UploadAsync(options, remotePath, uploadStream);
        }

        var expectedLocalPath = Path.Combine(fixture.RootPath, "data", "inbound", "world.txt");
        Assert.True(File.Exists(expectedLocalPath));
        Assert.Equal(payload, await File.ReadAllTextAsync(expectedLocalPath));

        var downloaded = await _client.DownloadAsync(options, remotePath);
        Assert.Equal(payload, Encoding.UTF8.GetString(downloaded));

        var entries = await _client.ListAsync(options, "/data/inbound");
        Assert.Contains("world.txt", entries);
    }
}


