using System.Net;
using System.Net.Sockets;

namespace SendSftpTestStudy.Tests.Infrastructure;

/// <summary>
/// テスト用に利用可能なポート番号を動的に取得するためのヘルパークラス
/// FTP版と同一の機能を提供し、SFTPテスト用サーバーのポート競合を回避する
/// </summary>
internal static class PortHelper
{
    /// <summary>
    /// システムで現在利用可能なポート番号を動的に取得する
    /// OSが自動割り当てする仕組みを利用して、複数のSFTPテストが同時実行されても競合しない
    /// </summary>
    /// <returns>利用可能なポート番号</returns>
    public static int GetAvailablePort()
    {
        // ポート番号0を指定してTcpListenerを作成（OSが自動で利用可能ポートを割り当て）
        var listener = new TcpListener(IPAddress.Loopback, 0);

        // リスナーを開始して実際のポート番号を確定
        listener.Start();

        // 割り当てられたポート番号を取得
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;

        // SFTPテスト用サーバーで使用するためリスナーを停止
        listener.Stop();

        return port;
    }
}

