using FubarDev.FtpServer;
using FubarDev.FtpServer.AccountManagement;
using FubarDev.FtpServer.FileSystem.DotNet;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace SendFtpTestStudy.Tests.Infrastructure;

public sealed class FtpServerFixture : IAsyncLifetime
{
    private const string TEST_USERNAME = "tester";
    private const string TEST_PASSWORD = "test-pass";

    private ServiceProvider? _provider;
    private IFtpServerHost? _host;

    public string RootPath { get; private set; } = null!;

    public int Port { get; private set; }

    public FtpConnectionOptions Options => new(
        FtpProtocol.Ftp,
        "127.0.0.1",
        Port,
        TEST_USERNAME,
        TEST_PASSWORD);

    public async Task InitializeAsync()
    {
        RootPath = Path.Combine(Path.GetTempPath(), "SendFtpTestStudy", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(RootPath);

        Port = PortHelper.GetAvailablePort();

        var services = new ServiceCollection();
        services.Configure<DotNetFileSystemOptions>(opt => opt.RootPath = RootPath);
        var membershipProvider = new SingleUserMembershipProvider(TEST_USERNAME, TEST_PASSWORD);
        services.AddSingleton<IMembershipProvider>(membershipProvider);
        services.AddSingleton<IMembershipProviderAsync>(membershipProvider);
        services.AddFtpServer(builder => builder
            .UseDotNetFileSystem());
        services.Configure<FtpServerOptions>(opt =>
        {
            opt.ServerAddress = "127.0.0.1";
            opt.Port = Port;
        });

        _provider = services.BuildServiceProvider();
        _host = _provider.GetRequiredService<IFtpServerHost>();
        await _host.StartAsync(CancellationToken.None).ConfigureAwait(false);
    }

    public async Task DisposeAsync()
    {
        if (_host is not null)
        {
            await _host.StopAsync(CancellationToken.None).ConfigureAwait(false);
        }

        if (_provider is not null)
        {
            await _provider.DisposeAsync();
        }

        if (!string.IsNullOrEmpty(RootPath) && Directory.Exists(RootPath))
        {
            Directory.Delete(RootPath, recursive: true);
        }
    }

    private sealed class SingleUserMembershipProvider(string username, string password) : IMembershipProviderAsync
    {
        public Task<MemberValidationResult> ValidateUserAsync(
            string username1,
            string password1,
            CancellationToken cancellationToken)
        {
            if (string.Equals(username1, username, StringComparison.Ordinal) &&
                string.Equals(password1, password, StringComparison.Ordinal))
            {
                var identity = new ClaimsIdentity(authenticationType: "ftp-basic");
                identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, username1));
                var principal = new ClaimsPrincipal(identity);
                return Task.FromResult(new MemberValidationResult(MemberValidationStatus.AuthenticatedUser, principal));
            }

            return Task.FromResult(new MemberValidationResult(MemberValidationStatus.InvalidLogin));
        }

        public Task LogOutAsync(ClaimsPrincipal principal, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task<MemberValidationResult> ValidateUserAsync(string userName, string passWord)
        {
            return ValidateUserAsync(userName, passWord, CancellationToken.None);
        }
    }
}


