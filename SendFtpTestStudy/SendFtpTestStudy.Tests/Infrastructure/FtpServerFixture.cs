using FubarDev.FtpServer;
using FubarDev.FtpServer.AccountManagement;
using FubarDev.FtpServer.FileSystem.DotNet;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace SendFtpTestStudy.Tests.Infrastructure;

/// <summary>
/// FTPサーバーのテスト用フィクスチャクラス
/// FubarDev.FtpServerを使用してテスト用のインメモリFTPサーバーを起動し、テスト終了時にクリーンアップを行う
/// xUnitのIAsyncLifetimeインターフェイスを実装して非同期初期化と破棄をサポート
/// </summary>
public sealed class FtpServerFixture : IAsyncLifetime
{
    /// <summary>
    /// テスト用のFTPユーザー名。ハードコーディングした値を使用。
    /// </summary>
    private const string TEST_USERNAME = "tester";

    /// <summary>
    /// テスト用のFTPパスワード。ハードコーディングした値を使用。
    /// </summary>
    private const string TEST_PASSWORD = "test-pass";

    /// <summary>
    /// 依存性注入コンテナ。FTPサーバーのサービスを管理する。
    /// </summary>
    private ServiceProvider? _provider;

    /// <summary>
    /// FTPサーバーのホストインスタンス。サーバーの開始と停止を制御する。
    /// </summary>
    private IFtpServerHost? _host;

    /// <summary>
    /// FTPサーバーのルートディレクトリのパス。一時ディレクトリ内に作成される。
    /// </summary>
    public string RootPath { get; private set; } = null!;

    /// <summary>
    /// FTPサーバーがリッスンするポート番号。PortHelperによって動的に割り当てられる。
    /// </summary>
    public int Port { get; private set; }

    /// <summary>
    /// テスト用FTPクライアントの接続オプション。
    /// ローカルホスト、動的ポート、固定のテスト用認証情報を使用。
    /// </summary>
    public FtpConnectionOptions Options => new(
        "127.0.0.1",
        Port,
        TEST_USERNAME,
        TEST_PASSWORD);

    /// <summary>
    /// FTPサーバーの初期化を非同期で実行する。
    /// 一時ディレクトリの作成、ポートの割り当て、FTPサーバーの構成と開始を行う。
    /// </summary>
    /// <returns>初期化タスク</returns>
    public async Task InitializeAsync()
    {
        // ユニークな一時ディレクトリを作成（テスト関競合回避のためGUIDを使用）
        RootPath = Path.Combine(Path.GetTempPath(), "SendFtpTestStudy", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(RootPath);
        
        // テストで使用するuploadsディレクトリを事前に作成
        Directory.CreateDirectory(Path.Combine(RootPath, "uploads"));

        // 利用可能なポートを取得
        Port = PortHelper.GetAvailablePort();

        // 依存性注入コンテナの構成
        var services = new ServiceCollection();

        // .NETファイルシステムをFTPサーバーのバックエンドとして使用
        services.Configure<DotNetFileSystemOptions>(opt => opt.RootPath = RootPath);

        // シングルユーザー認証プロバイダーを作成して登録
        var membershipProvider = new SingleUserMembershipProvider(TEST_USERNAME, TEST_PASSWORD);
        services.AddSingleton<IMembershipProvider>(membershipProvider);
        services.AddSingleton<IMembershipProviderAsync>(membershipProvider);

        // FTPサーバーサービスを追加
        services.AddFtpServer(builder => builder
            .UseDotNetFileSystem());

        // FTPサーバーのアドレスとポートを構成
        services.Configure<FtpServerOptions>(opt =>
        {
            opt.ServerAddress = "127.0.0.1";
            opt.Port = Port;
        });

        // サービスプロバイダーを構築してFTPサーバーホストを取得
        _provider = services.BuildServiceProvider();
        _host = _provider.GetRequiredService<IFtpServerHost>();

        // FTPサーバーを開始
        await _host.StartAsync(CancellationToken.None).ConfigureAwait(false);
    }

    /// <summary>
    /// FTPサーバーのクリーンアップを非同期で実行する。
    /// サーバーの停止、サービスプロバイダーの破棄、一時ディレクトリの削除を順次実行。
    /// </summary>
    /// <returns>クリーンアップタスク</returns>
    public async Task DisposeAsync()
    {
        // FTPサーバーホストが存在する場合は停止
        if (_host is not null)
        {
            await _host.StopAsync(CancellationToken.None).ConfigureAwait(false);
        }

        // サービスプロバイダーが存在する場合は破棄
        if (_provider is not null)
        {
            await _provider.DisposeAsync();
        }

        // 一時ディレクトリが存在する場合は再帰的に削除
        if (!string.IsNullOrEmpty(RootPath) && Directory.Exists(RootPath))
        {
            Directory.Delete(RootPath, recursive: true);
        }
    }

    /// <summary>
    /// テスト用のシンプルなユーザー認証プロバイダー。
    /// 単一のユーザー名とパスワードの組み合わせのみを受け入れる。
    /// プライマリコンストラクタで認証情報を受け取り、IMembershipProviderAsyncを実装している。
    /// </summary>
    /// <param name="username">許可されたユーザー名</param>
    /// <param name="password">許可されたパスワード</param>
    private sealed class SingleUserMembershipProvider(string username, string password) : IMembershipProviderAsync
    {
        /// <summary>
        /// ユーザー認証を非同期で実行する。
        /// コンストラクタで設定されたユーザー名とパスワードの組み合わせのみを受け入れる。
        /// </summary>
        /// <param name="username1">認証したいユーザー名</param>
        /// <param name="password1">認証したいパスワード</param>
        /// <param name="cancellationToken">キャンセルトークン</param>
        /// <returns>認証結果を含むMemberValidationResult</returns>
        public Task<MemberValidationResult> ValidateUserAsync(
            string username1,
            string password1,
            CancellationToken cancellationToken)
        {
            // ユーザー名とパスワードの両方がコンストラクタで設定された値と一致するかチェック
            if (string.Equals(username1, username, StringComparison.Ordinal) &&
                string.Equals(password1, password, StringComparison.Ordinal))
            {
                // 認証成功時はClaimsベースのIDを作成
                var identity = new ClaimsIdentity(authenticationType: "ftp-basic");
                identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, username1));
                var principal = new ClaimsPrincipal(identity);
                return Task.FromResult(new MemberValidationResult(MemberValidationStatus.AuthenticatedUser, principal));
            }

            // 認証失敗時は無効ログイン状態を返す
            return Task.FromResult(new MemberValidationResult(MemberValidationStatus.InvalidLogin));
        }

        /// <summary>
        /// ユーザーのログアウト処理を非同期で実行する。
        /// テスト用のシンプルな実装なので、特別な処理は行わない。
        /// </summary>
        /// <param name="principal">ログアウトするユーザーのプリンシパル</param>
        /// <param name="cancellationToken">キャンセルトークン</param>
        /// <returns>完了済みタスク</returns>
        public Task LogOutAsync(ClaimsPrincipal principal, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// ユーザー認証を実行する（キャンセルトークンなし版）。
        /// 内部的にはCancellationToken.Noneを使用して上記のメソッドを呼び出す。
        /// </summary>
        /// <param name="userName">認証したいユーザー名</param>
        /// <param name="passWord">認証したいパスワード</param>
        /// <returns>認証結果を含むMemberValidationResult</returns>
        public Task<MemberValidationResult> ValidateUserAsync(string userName, string passWord)
        {
            return ValidateUserAsync(userName, passWord, CancellationToken.None);
        }
    }
}
