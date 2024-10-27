using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    private static GraphServiceClient _graphClient;

    static async Task Main(string[] args)
    {
        // Microsoft 365の共通エンドポイントを使用
        string[] scopes = new[] { "https://graph.microsoft.com/.default" };

        // PublicClientApplicationを初期化
        var app = PublicClientApplicationBuilder
            .Create("14d82eec-204b-4c2f-b7e8-296a70dab67e") // Microsoft GraphのクライアントID
            .WithAuthority(AzureCloudInstance.AzurePublic, "common")
            .WithDefaultRedirectUri()
            .Build();

        try
        {
            // デバイスコードフローで認証
            var result = await app.AcquireTokenWithDeviceCode(scopes, deviceCodeResult =>
            {
                Console.WriteLine(deviceCodeResult.Message);
                // ユーザーが認証を完了するのを待つ
                Console.WriteLine("認証が完了したら、何かキーを押してください...");
                Console.ReadKey();
                return Task.CompletedTask;
            }).ExecuteAsync();

            Console.WriteLine("認証が完了しました。");

            //// カスタム認証プロバイダーを作成
            //var authProvider = new CustomAuthenticationProvider(result.AccessToken);

            //// GraphServiceClientを初期化
            //_graphClient = new GraphServiceClient(authProvider);

            //// 最近使用したファイルを取得
            //var recentItems = await _graphClient.Me.Drive.Recent().GetAsync();

            //// 結果を表示
            //if (recentItems?.Value != null)
            //{
            //    foreach (var item in recentItems.Value)
            //    {
            //        Console.WriteLine($"Name: {item.Name}, Last Modified: {item.LastModifiedDateTime}");
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("No recent items found or unable to retrieve the list.");
            //}
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        }
    }
}

// カスタム認証プロバイダークラス
public class CustomAuthenticationProvider : IAuthenticationProvider
{
    private readonly string _accessToken;

    public CustomAuthenticationProvider(string accessToken)
    {
        _accessToken = accessToken;
    }

    public Task AuthenticateRequestAsync(RequestInformation request, Dictionary<string, object> additionalAuthenticationContext = null, CancellationToken cancellationToken = default)
    {
        request.Headers.Add("Authorization", $"Bearer {_accessToken}");
        return Task.CompletedTask;
    }
}