using System.Text;
using SendFtpTestStudy.Tests.Infrastructure;

namespace SendFtpTestStudy.Tests;

public class FtpClientSftpTests(SftpServerFixture fixture) : IClassFixture<SftpServerFixture>
{
    private readonly FtpClient _client = new();

    [Fact]
    public async Task UploadOverSftp()
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

    }
}