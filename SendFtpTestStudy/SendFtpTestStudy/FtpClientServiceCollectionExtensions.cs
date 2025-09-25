using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace SendFtpTestStudy;

/// <summary>
/// IServiceCollectionの拡張メソッドを提供するクラス
/// FtpClientプロバイダーのDI登録を簡素化する
/// </summary>
public static class FtpClientServiceCollectionExtensions
{
    /// <summary>
    /// FtpClientプロバイダーをサービスコンテナに登録する
    /// </summary>
    /// <param name="services">サービスコレクション</param>
    /// <param name="configuration">設定オブジェクト（appsettings.jsonから読み込み）</param>
    /// <param name="configSectionName">設定セクション名（デフォルト: "FtpConnection"）</param>
    /// <returns>更新されたサービスコレクション</returns>
    public static IServiceCollection AddFtpClient(
        this IServiceCollection services,
        IConfiguration configuration,
        string configSectionName)
    {
        // FtpConnectionOptionsを設定セクションから読み込んでOptions登録
        services.Configure<FtpConnectionOptions>(
            configuration.GetSection(configSectionName));

        // IFtpClientProviderとして実装クラスを登録
        services.AddScoped<IFtpClientProvider, FtpClientProvider>();

        return services;
    }
}