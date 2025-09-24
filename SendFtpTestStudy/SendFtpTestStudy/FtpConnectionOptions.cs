namespace SendFtpTestStudy;

public enum FtpProtocol
{
    Ftp,
    Sftp
}

public sealed class FtpConnectionOptions
{
    public FtpConnectionOptions(
        FtpProtocol protocol,
        string host,
        int port,
        string username,
        string password,
        bool acceptAnyCertificate = false,
        bool acceptAnySshHostKey = false)
    {
        Protocol = protocol;
        Host = host ?? throw new ArgumentNullException(nameof(host));
        Port = port;
        Username = username ?? throw new ArgumentNullException(nameof(username));
        Password = password ?? throw new ArgumentNullException(nameof(password));
        AcceptAnyCertificate = acceptAnyCertificate;
        AcceptAnySshHostKey = acceptAnySshHostKey;
    }

    public FtpProtocol Protocol { get; }

    public string Host { get; }

    public int Port { get; }

    public string Username { get; }

    public string Password { get; }

    public bool AcceptAnyCertificate { get; }

    public bool AcceptAnySshHostKey { get; }
}
