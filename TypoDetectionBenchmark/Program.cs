using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace TypoDetectionBenchmark;

public record TestCase(int Id, bool IsTypo, string Category, string Text);

public record CorrectionResult(
    int Id,
    string Model,
    bool GroundTruth,
    string Category,
    string OriginalText,
    string CorrectedText,
    bool WasModified,
    long LatencyMs,
    int PromptTokens,
    int CompletionTokens,
    int ReasoningTokens,
    int TotalTokens,
    double CostUsd,
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
        Console.WriteLine(" 誤字脱字補正ベンチマーク: gpt-6-luna vs gpt-5.6-luna");
        Console.WriteLine($" テストデータ件数: {TestCases.Length} 件 (正常文 15件 / 誤字脱字文 15件)");
        Console.WriteLine("================================================================================");

        string openAiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
            ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User)
            ?? throw new InvalidOperationException("OPENAI_API_KEY not found.");

        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };

        var luna6Results = new List<CorrectionResult>();
        var luna56Results = new List<CorrectionResult>();

        Console.WriteLine("\n[1/2] gpt-6-luna の補正測定開始...");
        for (int i = 0; i < TestCases.Length; i++)
        {
            var tc = TestCases[i];
            Console.Write($"[{i + 1:D2}/30] ID:{tc.Id:D2} (Typo={tc.IsTypo, -5}) ... ");
            var res = await CorrectTextAsync(http, openAiKey, "gpt-6-luna", tc, 0.10, 0.50);
            luna6Results.Add(res);
            Console.WriteLine($"{res.LatencyMs,4}ms | In:{res.PromptTokens,3} Out:{res.CompletionTokens,3} (R:{res.ReasoningTokens,3}) | Mod={res.WasModified}");
            await Task.Delay(100);
        }

        Console.WriteLine("\n[2/2] gpt-5.6-luna の補正測定開始...");
        for (int i = 0; i < TestCases.Length; i++)
        {
            var tc = TestCases[i];
            Console.Write($"[{i + 1:D2}/30] ID:{tc.Id:D2} (Typo={tc.IsTypo, -5}) ... ");
            var res = await CorrectTextAsync(http, openAiKey, "gpt-5.6-luna", tc, 0.20, 1.20);
            luna56Results.Add(res);
            Console.WriteLine($"{res.LatencyMs,4}ms | In:{res.PromptTokens,3} Out:{res.CompletionTokens,3} (R:{res.ReasoningTokens,3}) | Mod={res.WasModified}");
            await Task.Delay(100);
        }

        Console.WriteLine("\n=== 集計結果 ===");
        PrintSummary("gpt-6-luna", luna6Results, 0.10, 0.50, 250_000);
        PrintSummary("gpt-5.6-luna", luna56Results, 0.20, 1.20, 2_500_000);

        SaveCsv("correction_benchmark_results.csv", luna6Results, luna56Results);
        Console.WriteLine("\n結果を correction_benchmark_results.csv に保存しました。");
    }

    private static async Task<CorrectionResult> CorrectTextAsync(
        HttpClient http, string apiKey, string model, TestCase tc, double inputPricePerM, double outputPricePerM)
    {
        try
        {
            using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var body = new
            {
                model = model,
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content = "あなたは高精度な日本語文章校正AIです。入力された文章を検査し、誤字・脱字・変換ミス・助詞の脱落や重複などの誤りがあれば修正してください。原文の文体・ニュアンス・表現は極力そのまま維持し、必要最小限の修正を行ってください。解説や前置きは一切出力せず、修正後の文章のみを出力してください。誤字脱字がない場合は、入力文章をそのまま完全一致で出力してください。"
                    },
                    new
                    {
                        role = "user",
                        content = tc.Text
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
                return new CorrectionResult(tc.Id, model, tc.IsTypo, tc.Category, tc.Text, "", false, sw.ElapsedMilliseconds, 0, 0, 0, 0, 0, false, err);
            }

            var json = await res.Content.ReadAsStringAsync();
            var doc = JsonNode.Parse(json);
            var reply = doc?["choices"]?[0]?["message"]?["content"]?.ToString().Trim() ?? "";
            int promptTok = doc?["usage"]?["prompt_tokens"]?.GetValue<int>() ?? 0;
            int compTok = doc?["usage"]?["completion_tokens"]?.GetValue<int>() ?? 0;
            int reasonTok = doc?["usage"]?["completion_tokens_details"]?["reasoning_tokens"]?.GetValue<int>() ?? 0;
            int totalTok = doc?["usage"]?["total_tokens"]?.GetValue<int>() ?? (promptTok + compTok);

            double cost = (promptTok * inputPricePerM / 1_000_000.0) + (compTok * outputPricePerM / 1_000_000.0);
            bool wasModified = !string.Equals(tc.Text.Trim(), reply, StringComparison.Ordinal);

            return new CorrectionResult(tc.Id, model, tc.IsTypo, tc.Category, tc.Text, reply, wasModified, sw.ElapsedMilliseconds, promptTok, compTok, reasonTok, totalTok, cost, true, null);
        }
        catch (Exception ex)
        {
            return new CorrectionResult(tc.Id, model, tc.IsTypo, tc.Category, tc.Text, "", false, 0, 0, 0, 0, 0, 0, false, ex.Message);
        }
    }

    private static void PrintSummary(string modelName, List<CorrectionResult> list, double inPrice, double outPrice, int freeQuota)
    {
        var valid = list.Where(x => x.IsSuccess).ToList();
        if (valid.Count == 0) return;

        var normals = valid.Where(x => !x.GroundTruth).ToList();
        var typos = valid.Where(x => x.GroundTruth).ToList();

        // 正常文のうち、変更されなかった割合（過剰修正なし率）
        int normalPreserved = normals.Count(x => !x.WasModified);
        // 誤字文のうち、変更された割合（修正実行率）
        int typoModified = typos.Count(x => x.WasModified);

        var latencies = valid.Select(x => (double)x.LatencyMs).OrderBy(x => x).ToList();
        double avgLat = latencies.Average();
        double medLat = latencies[latencies.Count / 2];
        double minLat = latencies.First();
        double maxLat = latencies.Last();
        double p90Lat = latencies[(int)(latencies.Count * 0.9)];
        double p95Lat = latencies[(int)(latencies.Count * 0.95)];

        double avgPromptTok = valid.Average(x => x.PromptTokens);
        double avgCompTok = valid.Average(x => x.CompletionTokens);
        double avgReasonTok = valid.Average(x => x.ReasoningTokens);
        double avgTotalTok = valid.Average(x => x.TotalTokens);
        double totalCost = valid.Sum(x => x.CostUsd);
        double avgCost = valid.Average(x => x.CostUsd);

        int maxFreeRequests = (int)(freeQuota / avgTotalTok);

        Console.WriteLine("--------------------------------------------------------------------------------");
        Console.WriteLine($"■ モデル: {modelName}");
        Console.WriteLine("--------------------------------------------------------------------------------");
        Console.WriteLine($"【補正挙動・品質】");
        Console.WriteLine($"  正常文の維持率 (過剰修正なし) : {(double)normalPreserved / normals.Count:P1} ({normalPreserved}/{normals.Count})");
        Console.WriteLine($"  誤字文の修正着手率 (変更検知) : {(double)typoModified / typos.Count:P1} ({typoModified}/{typos.Count})");
        Console.WriteLine($"【レスポンスタイム (Latency)】");
        Console.WriteLine($"  平均 (Mean)   : {avgLat:F1} ms");
        Console.WriteLine($"  中央値 (Median): {medLat:F1} ms");
        Console.WriteLine($"  最小 (Min)    : {minLat:F1} ms");
        Console.WriteLine($"  最大 (Max)    : {maxLat:F1} ms");
        Console.WriteLine($"  P90           : {p90Lat:F1} ms");
        Console.WriteLine($"  P95           : {p95Lat:F1} ms");
        Console.WriteLine($"【トークン消費量 (平均)】");
        Console.WriteLine($"  入力 (Prompt) : {avgPromptTok:F1} tokens");
        Console.WriteLine($"  出力 (Comp)   : {avgCompTok:F1} tokens (うち推論思考: {avgReasonTok:F1} tokens)");
        Console.WriteLine($"  合計 (Total)  : {avgTotalTok:F1} tokens");
        Console.WriteLine($"【コスト評価】");
        Console.WriteLine($"  単価設定      : 入力 ${inPrice:F2} / Mtok, 出力 ${outPrice:F2} / Mtok");
        Console.WriteLine($"  1件あたり平均 : ${avgCost:F6} (約 {avgCost * 155:F4} 円)");
        Console.WriteLine($"  30件合計費用  : ${totalCost:F6} (約 {totalCost * 155:F3} 円)");
        Console.WriteLine($"  日次無料枠    : {freeQuota:N0} tokens/day (Build ティア)");
        Console.WriteLine($"  完全無償可能数: 1日あたり 約 {maxFreeRequests:N0} 件");
    }

    private static void SaveCsv(string path, List<CorrectionResult> l6, List<CorrectionResult> l56)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Id,GroundTruth,Category,OriginalText," +
                      "Luna6_Modified,Luna6_LatencyMs,Luna6_PromptTok,Luna6_CompTok,Luna6_ReasonTok,Luna6_TotalTok,Luna6_CostUsd,Luna6_Output," +
                      "Luna56_Modified,Luna56_LatencyMs,Luna56_PromptTok,Luna56_CompTok,Luna56_ReasonTok,Luna56_TotalTok,Luna56_CostUsd,Luna56_Output");

        for (int i = 0; i < TestCases.Length; i++)
        {
            var tc = TestCases[i];
            var r6 = l6.FirstOrDefault(x => x.Id == tc.Id);
            var r56 = l56.FirstOrDefault(x => x.Id == tc.Id);

            sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                "{0},{1},\"{2}\",\"{3}\",{4},{5},{6},{7},{8},{9},{10:F6},\"{11}\",{12},{13},{14},{15},{16},{17},{18:F6},\"{19}\"",
                tc.Id,
                tc.IsTypo,
                tc.Category,
                tc.Text.Replace("\"", "\"\""),
                r6?.WasModified ?? false,
                r6?.LatencyMs ?? 0,
                r6?.PromptTokens ?? 0,
                r6?.CompletionTokens ?? 0,
                r6?.ReasoningTokens ?? 0,
                r6?.TotalTokens ?? 0,
                r6?.CostUsd ?? 0,
                (r6?.CorrectedText ?? "").Replace("\"", "\"\""),
                r56?.WasModified ?? false,
                r56?.LatencyMs ?? 0,
                r56?.PromptTokens ?? 0,
                r56?.CompletionTokens ?? 0,
                r56?.ReasoningTokens ?? 0,
                r56?.TotalTokens ?? 0,
                r56?.CostUsd ?? 0,
                (r56?.CorrectedText ?? "").Replace("\"", "\"\"")
            ));
        }

        File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
    }
}
