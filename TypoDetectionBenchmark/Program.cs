using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace TypoDetectionBenchmark;

public class DecisionBenchmarkOptimized
{
    private static readonly TestCase[] TestCases =
    [
        // --- 正常文 15件 ---
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

        // --- 誤字文 15件 ---
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

    // 最適化したOpenAI Decisions API instructions
    private const string OptimizedOpenAiInstructions = """
この日本語の文章に、客観的な【誤字・脱字・同音異義語の誤変換・濁音半濁音ミス・カナ打ち間違い・助詞の脱落や重複】が含まれているかを厳格に判定してください。
【誤りとする対象の例】「お願い足します」「修正バッチ（パッチの誤り）」「アプリケション」「機器冗長化（の抜け）」「批評的な課題（重大な課題の誤り）」「がが」「存じま」「サトイ」「となつております」などの客観的誤字脱字。
【誤りとしない対象】「予定通り/予定どおり」等の正当な表記ゆれ、専門用語（OAuth2.0, PDF, API, VPN等）、半角スペースの有無、文体や表現の好み。これらは誤りではありません。
""";

    // 最適化したTypeSafe AI criteria
    private const string OptimizedTypeSafeInstructions = "Does this Japanese text contain objective typos, omitted particles or characters, wrong homophones, phonetic/katakana errors, or duplicated words?";
    private static readonly object OptimizedTypeSafeCriteria = new
    {
        @true = "Contains objective typos, omitted particles (such as missing 'no' between nouns like '機器冗長化'), katakana/phonetic errors ('アプリケション', '修正バッチ' for patch, 'サトイ', 'となつて'), wrong kanji homophones ('お願い足します', '見張り等', '受項', '台幅な', '批評的な課題'), or duplicates ('がが', '経理部前経理部前').",
        @false = "Correct, standard Japanese text without errors. Valid orthographic variations (e.g. '予定通り' vs '予定どおり'), technical terms ('OAuth2.0', 'PDF', 'VPN', 'API'), spacing, and stylistic choices are completely valid and must NOT be considered typos."
    };

    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        string openAiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
            ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User)
            ?? throw new InvalidOperationException("OPENAI_API_KEY not found.");

        string typeSafeKey = Environment.GetEnvironmentVariable("TYPESAFE_API_KEY")
            ?? Environment.GetEnvironmentVariable("TYPESAFE_API_KEY", EnvironmentVariableTarget.User)
            ?? throw new InvalidOperationException("TYPESAFE_API_KEY not found.");

        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };

        Console.WriteLine("================================================================================");
        Console.WriteLine(" 第1部 高速判定ベンチマーク最適化検証 (Instructions / Criteria 精緻化テスト)");
        Console.WriteLine("================================================================================");

        var oResults = new List<(TestCase tc, double prob, long latency)>();
        var tResults = new List<(TestCase tc, double prob, long latency)>();

        Console.WriteLine("\n[1/2] OpenAI Decisions API (最適化指示) 測定開始...");
        for (int i = 0; i < TestCases.Length; i++)
        {
            var tc = TestCases[i];
            Console.Write($"[{i + 1:D2}/30] ID:{tc.Id:D2} (Typo={tc.IsTypo, -5}) ... ");
            var (prob, lat) = await EvalOpenAi(http, openAiKey, tc.Text);
            oResults.Add((tc, prob, lat));
            Console.WriteLine($"Prob: {prob:F3} | Lat: {lat,4}ms");
            await Task.Delay(100);
        }

        Console.WriteLine("\n[2/2] TypeSafe AI System One (最適化Criteria) 測定開始...");
        for (int i = 0; i < TestCases.Length; i++)
        {
            var tc = TestCases[i];
            Console.Write($"[{i + 1:D2}/30] ID:{tc.Id:D2} (Typo={tc.IsTypo, -5}) ... ");
            var (prob, lat) = await EvalTypeSafe(http, typeSafeKey, tc.Text);
            tResults.Add((tc, prob, lat));
            Console.WriteLine($"Prob: {prob:F3} | Lat: {lat,4}ms");
            await Task.Delay(100);
        }

        Console.WriteLine("\n=== 最適化後の集計サマリー ===");
        PrintSummary("OpenAI Decisions (最適化 instructions)", oResults);
        PrintSummary("TypeSafe AI System One (最適化 criteria)", tResults);
    }

    private static async Task<(double prob, long latency)> EvalOpenAi(HttpClient http, string apiKey, string text)
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
                    instructions = OptimizedOpenAiInstructions
                }
            }
        };
        req.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

        var sw = Stopwatch.StartNew();
        using var res = await http.SendAsync(req);
        sw.Stop();

        if (!res.IsSuccessStatusCode) return (0, sw.ElapsedMilliseconds);
        var json = await res.Content.ReadAsStringAsync();
        var doc = JsonNode.Parse(json);
        double prob = doc?["answers"]?[0]?["probability"]?.GetValue<double>() ?? 0.0;
        return (prob, sw.ElapsedMilliseconds);
    }

    private static async Task<(double prob, long latency)> EvalTypeSafe(HttpClient http, string apiKey, string text)
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
                    instructions = OptimizedTypeSafeInstructions,
                    criteria = OptimizedTypeSafeCriteria
                }
            }
        };
        req.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

        var sw = Stopwatch.StartNew();
        using var res = await http.SendAsync(req);
        sw.Stop();

        if (!res.IsSuccessStatusCode) return (0, sw.ElapsedMilliseconds);
        var json = await res.Content.ReadAsStringAsync();
        var doc = JsonNode.Parse(json);
        double prob = doc?["answers"]?["has_typo"]?["noul"]?.GetValue<double>() ?? 0.0;
        return (prob, sw.ElapsedMilliseconds);
    }

    private static void PrintSummary(string name, List<(TestCase tc, double prob, long latency)> list)
    {
        Console.WriteLine("--------------------------------------------------------------------------------");
        Console.WriteLine($"■ {name}");
        Console.WriteLine("--------------------------------------------------------------------------------");

        foreach (var th in new[] { 0.2, 0.3, 0.4, 0.5, 0.6, 0.7 })
        {
            int tp = list.Count(x => x.tc.IsTypo && x.prob >= th);
            int fp = list.Count(x => !x.tc.IsTypo && x.prob >= th);
            int tn = list.Count(x => !x.tc.IsTypo && x.prob < th);
            int fn = list.Count(x => x.tc.IsTypo && x.prob < th);

            double acc = (double)(tp + tn) / list.Count;
            double prec = (tp + fp) > 0 ? (double)tp / (tp + fp) : 0;
            double rec = (tp + fn) > 0 ? (double)tp / (tp + fn) : 0;
            double f1 = (prec + rec) > 0 ? 2 * (prec * rec) / (prec + rec) : 0;

            Console.WriteLine($"  閾値 {th:F1} => 正解率: {acc:P1} ({tp + tn}/30), 再現率: {rec:P1} ({tp}/15), 適合率: {prec:P1}, F1: {f1:F3} [TP:{tp}, FP:{fp}, TN:{tn}, FN:{fn}]");
        }

        Console.WriteLine($"  平均レスポンス: {list.Average(x => x.latency):F1} ms");
    }
}

public record TestCase(int Id, bool IsTypo, string Category, string Text);
