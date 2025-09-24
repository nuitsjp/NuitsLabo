using System.Text;
using SendSftpTestStudy;
using SendSftpTestStudy.Tests.Infrastructure;

namespace SendSftpTestStudy.Tests;

/// <summary>
/// SftpClientクラスの統合テストクラス
/// テスト用のSFTPサーバーを使用して、実際のSFTP通信を行いアップロード機能を検証する
/// FxSshライブラリを使用したSSH/SFTPサーバーとSSH.NETクライアントの組み合わせをテスト
/// xUnitのIClassFixtureを使用してテストクラス全体でSFTPサーバーを共有
/// </summary>
/// <param name="fixture">SFTPサーバーのテストフィクスチャ</param>
public class SftpClientTests(SftpServerFixture fixture) : IClassFixture<SftpServerFixture>
{
    /// <summary>
    /// テスト対象のSftpClientインスタンス。
    /// 各テストメソッドで共有され、テスト用SFTPサーバーへのアップロード機能をテストする。
    /// </summary>
    private readonly SftpClient _client = new();

    /// <summary>
    /// SFTPサーバーへのファイルアップロード機能をテストする
    /// テストシナリオ：
    /// 1. メモリストリームでテキストデータを作成
    /// 2. SftpClient.UploadAsyncでファイルをアップロード（ディレクトリ自動作成あり）
    /// 3. サーバーのローカルファイルシステムでファイルの存在と内容を検証
    /// FTPとは異なりSSHプロトコルでのセキュアなファイル転送を検証
    /// </summary>
    [Fact]
    public async Task UploadOverSftp()
    {
        // テスト用SFTPサーバーの接続オプションを取得（SSHホストキー検証無効化済み）
        var options = fixture.Options;

        // アップロード先のリモートパス（多層ディレクトリ構造でテスト）
        var remotePath = "/data/inbound/world.txt";

        // アップロードするテストデータ
        var payload = "Hello via SFTP";

        // メモリストリームでテストデータをラップしてアップロード実行
        await using (var uploadStream = new MemoryStream(Encoding.UTF8.GetBytes(payload)))
        {
            await _client.UploadAsync(options, remotePath, uploadStream);
        }

        // サーバーサイドのローカルファイルシステムでファイルの存在を検証
        var expectedLocalPath = Path.Combine(fixture.RootPath, "data", "inbound", "world.txt");
        Assert.True(File.Exists(expectedLocalPath));

        // アップロードされたファイルの内容が期待値と一致するか検証
        Assert.Equal(payload, await File.ReadAllTextAsync(expectedLocalPath));
    }
}
