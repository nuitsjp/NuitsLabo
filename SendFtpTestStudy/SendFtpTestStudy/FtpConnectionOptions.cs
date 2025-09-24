namespace SendFtpTestStudy;

public sealed class FtpConnectionOptions(
    FtpProtocol protocol,
    string host,
    int port,
    string username,
    string password,
    bool acceptAnyCertificate = false,
    bool acceptAnySshHostKey = false)
{
    public FtpProtocol Protocol { get; } = protocol;

    public string Host { get; } = host ?? throw new ArgumentNullException(nameof(host));

    public int Port { get; } = port;

    public string Username { get; } = username ?? throw new ArgumentNullException(nameof(username));

    public string Password { get; } = password ?? throw new ArgumentNullException(nameof(password));

    public bool AcceptAnyCertificate { get; } = acceptAnyCertificate;

    public bool AcceptAnySshHostKey { get; } = acceptAnySshHostKey;
}
