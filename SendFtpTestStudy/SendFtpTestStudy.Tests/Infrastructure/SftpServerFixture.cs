using FxSsh;
using FxSsh.Services;
using System.Net;

namespace SendFtpTestStudy.Tests.Infrastructure;

public sealed class SftpServerFixture : IAsyncLifetime
{
    private readonly object _lock = new();
    private SshServer? _server;
    private readonly List<FxSshSftpService> _services = [];

    public string RootPath { get; private set; } = null!;

    public int Port { get; private set; }

    public string Username { get; } = "testuser";

    public string Password { get; } = "testpass";

    public FtpConnectionOptions Options => new(
        FtpProtocol.Sftp,
        "127.0.0.1",
        Port,
        Username,
        Password,
        acceptAnySshHostKey: true);

    public Task InitializeAsync()
    {
        RootPath = Path.Combine(Path.GetTempPath(), "SendFtpTestStudy", "sftp", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(RootPath);

        Port = PortHelper.GetAvailablePort();

        var startingInfo = new StartingInfo(IPAddress.Loopback, Port, "SSH-2.0-FxSsh-Test");
        _server = new SshServer(startingInfo);
        _server.AddHostKey("rsa-sha2-256", KeyGenerator.GenerateRsaKeyPem(2048));
        _server.ConnectionAccepted += OnConnectionAccepted;
        _server.Start();

        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        if (_server is not null)
        {
            _server.ConnectionAccepted -= OnConnectionAccepted;
            _server.Stop();
            _server.Dispose();
        }

        lock (_lock)
        {
            foreach (var service in _services)
            {
                service.OnClose();
            }
            _services.Clear();
        }

        if (!string.IsNullOrEmpty(RootPath) && Directory.Exists(RootPath))
        {
            Directory.Delete(RootPath, recursive: true);
        }

        return Task.CompletedTask;
    }

    private void OnConnectionAccepted(object? sender, Session session)
    {
        session.ServiceRegistered += SessionOnServiceRegistered;
        session.Disconnected += (_, _) => session.ServiceRegistered -= SessionOnServiceRegistered;
    }

    private void SessionOnServiceRegistered(object? sender, SshService service)
    {
        switch (service)
        {
            case UserauthService userAuthService:
                userAuthService.Userauth += OnUserAuth;
                break;
            case ConnectionService connectionService:
                connectionService.CommandOpened += OnCommandOpened;
                break;
        }
    }

    private void OnUserAuth(object? sender, UserauthArgs e)
    {
        if (e.AuthMethod == "password" && e.Username == Username && e.Password == Password)
        {
            e.Result = true;
        }
    }

    private void OnCommandOpened(object? sender, CommandRequestedArgs e)
    {
        if (!string.Equals(e.ShellType, "subsystem", StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(e.CommandText, "sftp", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }


        var subsystem = new FxSshSftpService(RootPath);
        EventHandler<byte[]> outbound = (_, payload) => e.Channel.SendData(payload);
        subsystem.DataReceived += outbound;

        e.Channel.DataReceived += ChannelOnData;
        e.Channel.CloseReceived += ChannelOnClose;
        e.Channel.EofReceived += ChannelOnEof;

        lock (_lock)
        {
            _services.Add(subsystem);
        }

        Task.Run(() =>
        {
            try
            {
                subsystem.WaitForClose();
            }
            catch
            {
                // ignore cancellation related exceptions
            }
            finally
            {
                e.Channel.SendClose();
            }
        });

        void ChannelOnData(object? _, byte[] data) => subsystem.OnData(data);

        void ChannelOnEof(object? _, EventArgs __)
        {
            subsystem.OnClose();
        }

        void ChannelOnClose(object? _, EventArgs __)
        {
            subsystem.OnClose();
            e.Channel.DataReceived -= ChannelOnData;
            e.Channel.CloseReceived -= ChannelOnClose;
            e.Channel.EofReceived -= ChannelOnEof;
            subsystem.DataReceived -= outbound;

            lock (_lock)
            {
                _services.Remove(subsystem);
            }
        }
    }
}












