using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace TypoDetectionBenchmark;

public record TestCase(int Id, bool IsTypo, string Category, string Text);

public record ApiResult(
    int Id,
    string Model,
    bool GroundTruth,
    double Probability,
    bool PredictedTypo,
    long LatencyMs,
    bool IsSuccess,
    string? ErrorMessage
);

public class Program
{
    private static readonly TestCase[] TestCases =
    [
        // --- 正常文 (誤字脱字なし: IsTypo = false) ---
        new(1, false, "業務連絡",
            "来週月曜日に予定されている第3四半期の全社キックオフミーティングですが、会場の都合により開始時刻が10時から10時半へと変更となりました。参加予定の皆様におかれましては、各自スケジュールの再確認をお願いいたします。資料は事前に共有フォルダへ格納済みです。"),
        new(2, false, "製品サポート",
            "平素より弊社クラウドサービスをご利用いただき誠にありがとうございます。本日発生いたしました一部リージョンでの接続遅延につきましては、ネットワーク機器の冗長化切り替えを実施し、14時30分現在、すべてのサービスが正常に稼働していることを確認いたしました。"),
        new(3, false, "仕様変更案内",
            "セキュリティポリシーの改定に伴い、来月1日より外部連携APIの認証方式がOAuth2.0に一本化されます。現在旧来のAPIトークンを利用されているクライアントアプリケーションにつきましては、期日までに新方式への移行作業を完了していただきますようお願い申し上げます。"),
        new(4, false, "プロジェクト進捗",
            "顧客向けポータルサイトのリニューアル案件ですが、フロントエンドの実装が順調に進んでおり、予定通り今週末より結合テスト環境での動作検証フェーズへ移行いたします。バックエンド側のAPI結合も完了しており、現時点でクリティカルな課題は発生しておりません。"),
        new(5, false, "総務通知",
            "オフィスビルの定期電気設備点検に伴い、今週日曜日の終日にわたり社内ネットワークおよび執務室の電源が一時停止いたします。当日は終日入館規制が実施されますので、休日出勤はご遠慮ください。リモートワーク環境からの社内VPN接続も利用不可となります。"),
        new(6, false, "採用連絡",
            "この度は弊社のキャリア採用選考にご応募いただき、誠にありがとうございました。慎重なる書類選考の結果、ぜひ次のステップとしてWeb適性検査および一次面接をご案内したく存じます。つきましては、下記の日程候補の中からご都合の良い日時をご選択ください。"),
        new(7, false, "不具合報告",
            "最新バージョン1.4.2にアップデートを行った環境において、PDF出力ボタンを押下した際にエラーダイアログが表示される事象が確認されました。ログ解析の結果、テンポラリフォルダのアクセス権限に起因することが判明したため、修正パッチの配布準備を進めています。"),
        new(8, false, "社内研修案内",
            "来月より全社員を対象とした情報セキュリティ基礎研修を実施いたします。本研修はeラーニング形式となっており、受講期間内に各自のペースで履修いただく形となります。最終確認テストにて80点以上の獲得が修了条件となりますので、期限内の受講をお願いします。"),
        new(9, false, "契約関連",
            "先日ご送付いただきました機密保持契約書（NDA）のドラフトにつきまして、法務部門による確認が完了いたしました。条項の一部に修正の要望がございますので、修正履歴を反映した差分ファイルを本メールに添付いたします。内容をご確認のうえご返信ください。"),
        new(10, false, "営業日報",
            "本日、株式会社A様を訪問し、新規導入予定の業務自動化ツールに関するデモンストレーションを実施いたしました。担当部署の部長様からも好意的な反応をいただいており、来週中に概算見積もりの提示とトライアル環境の提供を行う方向で話がまとまりました。"),
        new(11, false, "インフラ保守",
            "定期メンテナンス作業の一環として、深夜2時よりデータベースサーバーのインデックス再構築およびバックアップ検証を実施いたします。作業中は断続的なクエリ応答遅延が発生する可能性がありますが、システム全体の停止を伴うものではございません。"),
        new(12, false, "イベント告知",
            "来月開催予定の技術カンファレンスにおきまして、弊社エンジニアによるマイクロサービス移行事例の登壇セッションが決定いたしました。オンライン配信も同時に行われますので、ご興味のある方はイベント特設サイトより事前参加登録をお願いいたします。"),
        new(13, false, "経費精算通知",
            "当月分の経費精算申請の締め切りは、今週金曜日の18時までとなっております。領収書の原本提出が必要な案件につきましては、経理部前の専用提出ボックスへ投函をお願いいたします。期日を過ぎた申請は翌月分の処理となりますのでご注意ください。"),
        new(14, false, "人事通達",
            "新年度の人事異動および組織再編に伴う内示を社内ポータルサイトにて公開いたしました。対象となる社員の皆様には所属長より個別にご連絡を差し上げます。異動に伴う引継ぎ計画書の作成および提出期限は、来月中旬までとなっております。"),
        new(15, false, "リリース告知",
            "モバイルアプリの最新版バージョン2.0をApp StoreおよびGoogle Playにて公開いたしました。本アップデートではダークモードへの対応と起動速度の大幅な改善が含まれております。ユーザーの皆様には自動更新または手動でのアップデートを推奨いたします。"),

        // --- 誤字・脱字あり文 (IsTypo = true) ---
        new(16, true, "誤変換（同音異義語）",
            "来週月曜日に予定されている第3四半期の全社キックオフミーティングですが、会場の都合により開始時刻が10時から10時半へと変更となりました。参加予定の皆様におかれましては、各自スケジュールの再確認をお願い足します。資料は事前に共有フォルダへ格納済みです。"),
        new(17, true, "脱字（助詞抜け）",
            "平素より弊社クラウドサービスをご利用いただき誠にありがとうございます。本日発生いたしました一部リージョンでの接続遅延につきましては、ネットワーク機器冗長化切り替えを実施し、14時30分現在、すべてのサービスが正常に稼働していることを確認いたしました。"),
        new(18, true, "タイポ（促音抜け）",
            "セキュリティポリシーの改定に伴い、来月1日より外部連携APIの認証方式がOAuth2.0に一本化されます。現在旧来のAPIトークンを利用されているクライアントアプリケションにつきましては、期日までに新方式への移行作業を完了していただきますようお願い申し上げます。"),
        new(19, true, "誤字（漢字の間違い）",
            "顧客向けポータルサイトのリニューアル案件ですが、フロントエンドの実装が順調に進んでおり、予定通り今週末より結合テスト環境での動作検証フェーズへ移行いたします。バックエンド側のAPI結合も完了しており、現時点で批評的な課題は発生しておりません。"),
        new(20, true, "助詞の重複（重複入力）",
            "オフィスビルの定期電気設備点検に伴い、今週日曜日の終日にわたり社内ネットワークおよび執務室の電源が一時停止いたします。当日は終日入館規制がが実施されますので、休日出勤はご遠慮ください。リモートワーク環境からの社内VPN接続も利用不可となります。"),
        new(21, true, "脱字（語尾の脱落）",
            "この度は弊社のキャリア採用選考にご応募いただき、誠にありがとうございました。慎重なる書類選考の結果、ぜひ次のステップとしてWeb適性検査および一次面接をご案内したく存じま。つきましては、下記の日程候補の中からご都合の良い日時をご選択ください。"),
        new(22, true, "誤変換（同音異義語）",
            "最新バージョン1.4.2にアップデートを行った環境において、PDF出力ボタンを押下した際にエラーダイアログが表示される事象が確認されました。ログ解析の結果、テンポラリフォルダのアクセス権限に起因することが判明したため、修正バッチの配布準備を進めています。"),
        new(23, true, "タイポ（同音漢字ミス）",
            "来月より全社員を対象とした情報セキュリティ基礎研修を実施いたします。本研修はeラーニング形式となっており、受講期間内に各自のペースで履修いただく形となります。最終確認テストにて80点以上の獲得が修了条件となりますので、期限内の受項をお願いします。"),
        new(24, true, "送りがなの誤り",
            "先日ご送付いただきました機密保持契約書（NDA）のドラフトにつきまして、法務部門による確認が完了いたしました。条項の一部に修正の要望がございますので、修正履歴を反映した差分ファイルを本メールに添付いたします。内容をご確認のうえご返信下り。"),
        new(25, true, "誤変換（同音異義語）",
            "本日、株式会社A様を訪問し、新規導入予定の業務自動化ツールに関するデモンストレーションを実施いたしました。担当部署の部長様からも好意的な反応をいただいており、来週中に概算見張り等の提示とトライアル環境の提供を行う方向で話がまとまりました。"),
        new(26, true, "脱字（文字抜け）",
            "定期メンテナンス作業の一環として、深夜2時よりデータベースサーバーのインデックス再構築およびバックアップ検証を実施いたします。作業中は断続的なクエリ応答遅延が発生する能性がありますが、システム全体の停止を伴うものではございません。"),
        new(27, true, "タイポ（カナ入れ替わり）",
            "来月開催予定の技術カンファレンスにおきまして、弊社エンジニアによるマイクロサービス移行事例の登壇セッションが決定いたしました。オンライン配信も同時に行われますので、ご興味のある方はイベント特設サトイより事前参加登録をお願いいたします。"),
        new(28, true, "重複（単語重複）",
            "当月分の経費精算申請の締め切りは、今週金曜日の18時までとなっております。領収書の原本提出が必要な案件につきましては、経理部前経理部前の専用提出ボックスへ投函をお願いいたします。期日を過ぎた申請は翌月分の処理となりますのでご注意ください。"),
        new(29, true, "タイポ（促音小文字化漏れ）",
            "新年度の人事異動および組織再編に伴う内示を社内ポータルサイトにて公開いたしました。対象となる社員の皆様には所属長より個別にご連絡を差し上げます。異動に伴う引継ぎ計画書の作成および提出期限は、来月中旬までとなつております。"),
        new(30, true, "誤変換（同音異義語）",
            "モバイルアプリの最新版バージョン2.0をApp StoreおよびGoogle Playにて公開いたしました。本アップデートではダークモードへの対応と起動速度の台幅な改善が含まれております。ユーザーの皆様には自動更新または手動でのアップデートを推奨いたします。")
    ];

    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("================================================================================");
        Console.WriteLine(" 誤字脱字判定ベンチマーク: OpenAI Decisions API vs TypeSafe AI System One");
        Console.WriteLine($" テストデータ件数: {TestCases.Length} 件 (正常文 15件 / 誤字脱字文 15件, 各120〜126文字)");
        Console.WriteLine("================================================================================");

        string openAiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
            ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User)
            ?? throw new InvalidOperationException("OPENAI_API_KEY not found.");

        string typeSafeKey = Environment.GetEnvironmentVariable("TYPESAFE_API_KEY")
            ?? Environment.GetEnvironmentVariable("TYPESAFE_API_KEY", EnvironmentVariableTarget.User)
            ?? throw new InvalidOperationException("TYPESAFE_API_KEY not found.");

        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };

        // ウォームアップ
        Console.WriteLine("\n[1/3] ウォームアップ実行中...");
        await EvaluateOpenAiAsync(http, openAiKey, "ウォームアップテスト用の文章です。");
        await EvaluateTypeSafeAsync(http, typeSafeKey, "ウォームアップテスト用の文章です。");
        Console.WriteLine("ウォームアップ完了。\n");

        // ベンチマーク実行
        Console.WriteLine("[2/3] ベンチマーク測定開始 (全30件)...");
        var openAiResults = new List<ApiResult>();
        var typeSafeResults = new List<ApiResult>();

        for (int i = 0; i < TestCases.Length; i++)
        {
            var tc = TestCases[i];
            Console.Write($"[{i + 1:D2}/{TestCases.Length}] ID:{tc.Id:D2} (Typo={tc.IsTypo, -5}) ... ");

            // OpenAI Decisions
            var oRes = await EvaluateOpenAiAsync(http, openAiKey, tc.Text, tc.Id, tc.IsTypo);
            openAiResults.Add(oRes);

            // TypeSafe AI
            var tRes = await EvaluateTypeSafeAsync(http, typeSafeKey, tc.Text, tc.Id, tc.IsTypo);
            typeSafeResults.Add(tRes);

            Console.WriteLine(
                $"OpenAI: {oRes.Probability:F2} ({oRes.LatencyMs,4}ms, pred={oRes.PredictedTypo}) | " +
                $"TypeSafe: {tRes.Probability:F2} ({tRes.LatencyMs,4}ms, pred={tRes.PredictedTypo})"
            );

            // レートリミット配慮で微小待機
            await Task.Delay(100);
        }

        Console.WriteLine("\n[3/3] 結果集計・レポート作成中...");
        PrintSummary("OpenAI Decisions (gpt-6-luna)", openAiResults);
        PrintSummary("TypeSafe AI System One (jev-latest)", typeSafeResults);

        // CSV出力
        SaveCsv("benchmark_results.csv", openAiResults, typeSafeResults);
        Console.WriteLine("\n結果を benchmark_results.csv に保存しました。");
    }

    private static async Task<ApiResult> EvaluateOpenAiAsync(
        HttpClient http, string apiKey, string text, int id = 0, bool isTypo = false)
    {
        try
        {
            using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/decisions");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var body = new
            {
                model = "gpt-6-luna",
                input = text,
                questions = new[]
                {
                    new
                    {
                        type = "predicate",
                        name = "has_typo",
                        instructions = "この日本語の文章に、誤字、脱字、変換ミス、助詞の抜けや重複などの誤りが含まれているか判定してください。"
                    }
                }
            };
            req.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var sw = Stopwatch.StartNew();
            using var res = await http.SendAsync(req);
            sw.Stop();

            if (!res.IsSuccessStatusCode)
            {
                var err = await res.Content.ReadAsStringAsync();
                return new ApiResult(id, "OpenAI", isTypo, 0, false, sw.ElapsedMilliseconds, false, $"{(int)res.StatusCode}: {err}");
            }

            var json = await res.Content.ReadAsStringAsync();
            var doc = JsonNode.Parse(json);
            double prob = doc?["answers"]?[0]?["probability"]?.GetValue<double>() ?? 0.0;
            return new ApiResult(id, "OpenAI", isTypo, prob, prob >= 0.5, sw.ElapsedMilliseconds, true, null);
        }
        catch (Exception ex)
        {
            return new ApiResult(id, "OpenAI", isTypo, 0, false, 0, false, ex.Message);
        }
    }

    private static async Task<ApiResult> EvaluateTypeSafeAsync(
        HttpClient http, string apiKey, string text, int id = 0, bool isTypo = false)
    {
        try
        {
            using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.typesafe.ai/v1/systemone");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var body = new
            {
                model = "jev-latest",
                state = text,
                questions = new Dictionary<string, object>
                {
                    ["has_typo"] = new
                    {
                        type = "noul",
                        instructions = "Does this Japanese text contain any typos, omitted characters, wrong kanji/words, grammatical errors, or duplicates?",
                        criteria = new
                        {
                            @true = "Contains typos, omitted characters, wrong words, or grammatical errors",
                            @false = "Correct, standard, natural Japanese text without any errors"
                        }
                    }
                }
            };
            req.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var sw = Stopwatch.StartNew();
            using var res = await http.SendAsync(req);
            sw.Stop();

            if (!res.IsSuccessStatusCode)
            {
                var err = await res.Content.ReadAsStringAsync();
                return new ApiResult(id, "TypeSafe", isTypo, 0, false, sw.ElapsedMilliseconds, false, $"{(int)res.StatusCode}: {err}");
            }

            var json = await res.Content.ReadAsStringAsync();
            var doc = JsonNode.Parse(json);
            double prob = doc?["answers"]?["has_typo"]?["noul"]?.GetValue<double>() ?? 0.0;
            return new ApiResult(id, "TypeSafe", isTypo, prob, prob >= 0.5, sw.ElapsedMilliseconds, true, null);
        }
        catch (Exception ex)
        {
            return new ApiResult(id, "TypeSafe", isTypo, 0, false, 0, false, ex.Message);
        }
    }

    private static void PrintSummary(string title, List<ApiResult> results)
    {
        var valid = results.Where(r => r.IsSuccess).ToList();
        if (valid.Count == 0)
        {
            Console.WriteLine($"\n[{title}] 全リクエストが失敗しました。");
            return;
        }

        int tp = valid.Count(r => r.GroundTruth && r.PredictedTypo);
        int fp = valid.Count(r => !r.GroundTruth && r.PredictedTypo);
        int tn = valid.Count(r => !r.GroundTruth && !r.PredictedTypo);
        int fn = valid.Count(r => r.GroundTruth && !r.PredictedTypo);

        double accuracy = (double)(tp + tn) / valid.Count;
        double precision = (tp + fp) > 0 ? (double)tp / (tp + fp) : 0.0;
        double recall = (tp + fn) > 0 ? (double)tp / (tp + fn) : 0.0;
        double f1 = (precision + recall) > 0 ? 2 * (precision * recall) / (precision + recall) : 0.0;

        var latencies = valid.Select(r => (double)r.LatencyMs).OrderBy(x => x).ToList();
        double min = latencies.First();
        double max = latencies.Last();
        double avg = latencies.Average();
        double median = latencies[latencies.Count / 2];
        double p90 = latencies[(int)(latencies.Count * 0.9)];
        double p95 = latencies[(int)(latencies.Count * 0.95)];

        Console.WriteLine("\n--------------------------------------------------------------------------------");
        Console.WriteLine($"■ モデル: {title}");
        Console.WriteLine("--------------------------------------------------------------------------------");
        Console.WriteLine($"【精度 (閾値 0.5)】");
        Console.WriteLine($"  正解率 (Accuracy) : {accuracy:P1} ({tp + tn}/{valid.Count})");
        Console.WriteLine($"  適合率 (Precision): {precision:P1} (誤字と判定したうち実際に誤字だった割合)");
        Console.WriteLine($"  再現率 (Recall)   : {recall:P1} (実際の誤字のうち検知できた割合)");
        Console.WriteLine($"  F1 スコア         : {f1:F3}");
        Console.WriteLine($"  混同行列          : TP={tp}, FP={fp}, TN={tn}, FN={fn}");

        Console.WriteLine($"【反応速度 (Latency)】");
        Console.WriteLine($"  平均 (Mean)       : {avg:F1} ms");
        Console.WriteLine($"  中央値 (Median)   : {median:F1} ms");
        Console.WriteLine($"  最小 (Min)        : {min:F1} ms");
        Console.WriteLine($"  最大 (Max)        : {max:F1} ms");
        Console.WriteLine($"  90%タイル (P90)   : {p90:F1} ms");
        Console.WriteLine($"  95%タイル (P95)   : {p95:F1} ms");

        // 閾値感度分析
        Console.WriteLine($"【閾値感度 (Accuracy / F1)】");
        foreach (var th in new[] { 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8 })
        {
            int t_tp = valid.Count(r => r.GroundTruth && r.Probability >= th);
            int t_fp = valid.Count(r => !r.GroundTruth && r.Probability >= th);
            int t_tn = valid.Count(r => !r.GroundTruth && r.Probability < th);
            int t_fn = valid.Count(r => r.GroundTruth && r.Probability < th);

            double acc_th = (double)(t_tp + t_tn) / valid.Count;
            double prec_th = (t_tp + t_fp) > 0 ? (double)t_tp / (t_tp + t_fp) : 0.0;
            double rec_th = (t_tp + t_fn) > 0 ? (double)t_tp / (t_tp + t_fn) : 0.0;
            double f1_th = (prec_th + rec_th) > 0 ? 2 * (prec_th * rec_th) / (prec_th + rec_th) : 0.0;

            Console.WriteLine($"  閾値 {th:F1} => 正解率: {acc_th:P1}, 再現率: {rec_th:P1}, 適合率: {prec_th:P1}, F1: {f1_th:F3}");
        }
    }

    private static void SaveCsv(string path, List<ApiResult> oRes, List<ApiResult> tRes)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Id,GroundTruth,Category,TextLength,OpenAi_Prob,OpenAi_Pred,OpenAi_LatencyMs,TypeSafe_Prob,TypeSafe_Pred,TypeSafe_LatencyMs,Text");

        for (int i = 0; i < TestCases.Length; i++)
        {
            var tc = TestCases[i];
            var o = oRes.FirstOrDefault(x => x.Id == tc.Id);
            var t = tRes.FirstOrDefault(x => x.Id == tc.Id);

            sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                "{0},{1},\"{2}\",{3},{4:F3},{5},{6},{7:F3},{8},{9},\"{10}\"",
                tc.Id,
                tc.IsTypo,
                tc.Category,
                tc.Text.Length,
                o?.Probability ?? 0,
                o?.PredictedTypo ?? false,
                o?.LatencyMs ?? 0,
                t?.Probability ?? 0,
                t?.PredictedTypo ?? false,
                t?.LatencyMs ?? 0,
                tc.Text.Replace("\"", "\"\"")
            ));
        }

        File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
    }
}
