using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SendSftpTestStudy;

/// <summary>
/// IServiceCollection向けのSFTP関連登録ヘルパー
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// SftpClientProviderとSftpConnectionOptionsをサービスコレクションに登録する
    /// </summary>
    public static IServiceCollection AddSftpClientProvider(
        this IServiceCollection services,
        IConfiguration configuration,
        string configSectionName)
    {
        services.Configure<SftpConnectionOptions>(
            configuration.GetSection(configSectionName));

        services.AddScoped<ISftpClientProvider, SftpClientProvider>();

        return services;
    }
}
