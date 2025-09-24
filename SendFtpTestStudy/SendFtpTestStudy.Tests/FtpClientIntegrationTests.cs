using SendFtpTestStudy;
using System.Text;
using SendFtpTestStudy.Tests.Infrastructure;

namespace SendFtpTestStudy.Tests;

public class FtpClientFtpTests : IClassFixture<FtpServerFixture>
{
    private readonly FtpServerFixture _fixture;
    private readonly FtpClient _client = new();

    public FtpClientFtpTests(FtpServerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task UploadDownloadAndListOverFtp()
    {
        var options = _fixture.Options;
        var remotePath = "/uploads/hello.txt";
        var payload = "Hello via FTP";

        await using (var uploadStream = new MemoryStream(Encoding.UTF8.GetBytes(payload)))
        {
            await _client.UploadAsync(options, remotePath, uploadStream);
        }

        var expectedLocalPath = Path.Combine(_fixture.RootPath, "uploads", "hello.txt");
        Assert.True(File.Exists(expectedLocalPath));
        Assert.Equal(payload, await File.ReadAllTextAsync(expectedLocalPath));

        var downloaded = await _client.DownloadAsync(options, remotePath);
        Assert.Equal(payload, Encoding.UTF8.GetString(downloaded));

        var entries = await _client.ListAsync(options, "/uploads");
        Assert.Contains("hello.txt", entries);
    }
}

public class FtpClientSftpTests : IClassFixture<SftpServerFixture>
{
    private readonly SftpServerFixture _fixture;
    private readonly FtpClient _client = new();

    public FtpClientSftpTests(SftpServerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task UploadDownloadAndListOverSftp()
    {
        var options = _fixture.Options;
        var remotePath = "/data/inbound/world.txt";
        var payload = "Hello via SFTP";

        await using (var uploadStream = new MemoryStream(Encoding.UTF8.GetBytes(payload)))
        {
            await _client.UploadAsync(options, remotePath, uploadStream);
        }

        var expectedLocalPath = Path.Combine(_fixture.RootPath, "data", "inbound", "world.txt");
        Assert.True(File.Exists(expectedLocalPath));
        Assert.Equal(payload, await File.ReadAllTextAsync(expectedLocalPath));

        var downloaded = await _client.DownloadAsync(options, remotePath);
        Assert.Equal(payload, Encoding.UTF8.GetString(downloaded));

        var entries = await _client.ListAsync(options, "/data/inbound");
        Assert.Contains("world.txt", entries);
    }
}


