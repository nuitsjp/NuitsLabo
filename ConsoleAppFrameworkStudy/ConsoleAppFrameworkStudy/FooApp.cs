using ConsoleAppFramework;
using Microsoft.Extensions.Logging;

namespace ConsoleAppFrameworkStudy;

public class FooApp(IFooService fooService, ILogger<FooApp> logger)
{
    /// <summary>
    /// Serilogのコンソールログフォーマットのサンプル
    /// </summary>
    /// <param name="businessId">-b, 工程の業務ID</param>
    /// <param name="processSno">-p, 工程の工程枝番</param>
    /// <param name="unitId">-u, ユニットID</param>
    /// <param name="processNumber">-pn, プロセスNo.</param>
    [Command("")]
    public void Run(
        int businessId,
        int processSno,
        int unitId,
        int processNumber)
    {
        // アプリケーション開始ログ
        logger.LogInformation("=== Serilogコンソールフォーマットサンプルアプリケーション開始 ===");
        
        // 引数情報のログ出力
        logger.LogInformation("引数情報: businessId: {BusinessId}, processSno: {ProcessSno}, unitId: {UnitId}, processNumber: {ProcessNumber}", 
            businessId, processSno, unitId, processNumber);
        
        // 様々なログレベルのサンプル
        logger.LogTrace("これはTraceレベルのログです（通常は表示されません）");
        logger.LogDebug("これはDebugレベルのログです（通常は表示されません）");
        logger.LogInformation("これはInformationレベルのログです");
        logger.LogWarning("これはWarningレベルのログです - 注意が必要な状況");
        logger.LogError("これはErrorレベルのログです - エラーが発生しました");
        logger.LogCritical("これはCriticalレベルのログです - 致命的なエラー");
        
        // 構造化ログのサンプル
        var serviceInfo = new { Name = fooService.GetType().Name, Timestamp = DateTime.Now };
        logger.LogInformation("サービス情報: {@ServiceInfo}", serviceInfo);
        
        // 処理実行のサンプル
        logger.LogInformation("業務処理を実行中...");
        
        try
        {
            // 何らかの処理をシミュレート
            ProcessBusiness(businessId, processSno);
            logger.LogInformation("業務処理が正常に完了しました");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "業務処理中にエラーが発生しました");
        }
        
        logger.LogInformation("=== アプリケーション終了 ===");
    }
    
    private void ProcessBusiness(int businessId, int processSno)
    {
        logger.LogInformation("業務ID {BusinessId} の処理開始 (工程枝番: {ProcessSno})", businessId, processSno);
        
        // 処理時間をシミュレート
        Thread.Sleep(100);
        
        // 時々エラーを発生させてエラーログのサンプルを表示
        if (businessId % 10 == 0)
        {
            throw new InvalidOperationException($"業務ID {businessId} の処理でエラーが発生しました");
        }
        
        logger.LogInformation("業務ID {BusinessId} の処理完了", businessId);
    }
}

public interface IFooService;
public class FooService : IFooService;