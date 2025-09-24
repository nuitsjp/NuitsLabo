namespace SendSftpTestStudy;

public sealed class SftpConnectionOptions(
    string host,
    int port,
    string username,
    string password,
    bool acceptAnySshHostKey = false)
{
    public string Host { get; } = host ?? throw new ArgumentNullException(nameof(host));

    public int Port { get; } = port;

    public string Username { get; } = username ?? throw new ArgumentNullException(nameof(username));

    public string Password { get; } = password ?? throw new ArgumentNullException(nameof(password));

    public bool AcceptAnySshHostKey { get; } = acceptAnySshHostKey;
}