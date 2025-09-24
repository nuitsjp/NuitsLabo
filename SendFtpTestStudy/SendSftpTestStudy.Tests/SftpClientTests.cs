using System.Text;
using SendSftpTestStudy;
using SendSftpTestStudy.Tests.Infrastructure;

namespace SendSftpTestStudy.Tests;

public class SftpClientTests(SftpServerFixture fixture) : IClassFixture<SftpServerFixture>
{
    private readonly SftpClient _client = new();

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
