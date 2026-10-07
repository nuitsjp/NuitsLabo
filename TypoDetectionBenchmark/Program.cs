using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace TypoDetectionBenchmark;

public record TestCase(int Id, bool IsTypo, string Category, string Text);

public class PromptOptimization
{
    private static readonly TestCase[] TestCases =
    [
        // 正常文 15件
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

        // 誤字文 15件
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

    // 工夫したプロンプト (Prompt v2)
    private const string OptimizedSystemPrompt = """
あなたは厳格な日本語文章校正AIです。
入力された文章を検査し、明らかな【誤字・脱字・同音異義語の誤変換・カナ打ち間違い・助詞の脱落や重複】のみを最小限修正してください。

【厳守すべき絶対ルール】
1. 推敲・リライトの禁止:
   - より自然な表現への言い換え、語彙の変更、文章の改善・リライトは固く禁じます。
   - 例: 「獲得」を「取得」にする、「スケジュールの」を「のスケジュールの」にする等の一切の言い換えは禁止です。
2. 表記ゆれ・句読点変更の禁止:
   - 漢字とひらがなの使い分け（例: 「予定通り」と「予定どおり」）、半角スペースの有無（例: 「OAuth2.0」と「OAuth 2.0」）、読点の追加・削除は、誤字脱字ではないため絶対に変更してはなりません。
3. 原則完全維持:
   - 確信が持てる致命的な誤字脱字・文法崩壊がない場合は、一文字も変更せず入力文章と完全一致で出力してください。
4. 出力形式:
   - 前置き・解説・挨拶は一切含めず、修正後の文章（または完全一致の原文）のみを出力してください。
""";

    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("================================================================================");
        Console.WriteLine(" gpt-5.6-luna プロンプト最適化検証 (Prompt v2 による正常文維持率改善テスト)");
        Console.WriteLine("================================================================================");

        string openAiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
            ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User)
            ?? throw new InvalidOperationException("OPENAI_API_KEY not found.");

        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };

        var results = new List<(TestCase tc, string output, bool modified, long latency, int inTok, int outTok, int reasonTok)>();

        for (int i = 0; i < TestCases.Length; i++)
        {
            var tc = TestCases[i];
            Console.Write($"[{i + 1:D2}/30] ID:{tc.Id:D2} (Typo={tc.IsTypo, -5}) ... ");

            using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", openAiKey);
            var body = new
            {
                model = "gpt-5.6-luna",
                messages = new[]
                {
                    new { role = "system", content = OptimizedSystemPrompt },
                    new { role = "user", content = tc.Text }
                }
            };
            req.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var sw = Stopwatch.StartNew();
            using var res = await http.SendAsync(req);
            sw.Stop();

            if (!res.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {res.StatusCode}");
                continue;
            }

            var json = await res.Content.ReadAsStringAsync();
            var doc = JsonNode.Parse(json);
            var reply = doc?["choices"]?[0]?["message"]?["content"]?.ToString().Trim() ?? "";
            int promptTok = doc?["usage"]?["prompt_tokens"]?.GetValue<int>() ?? 0;
            int compTok = doc?["usage"]?["completion_tokens"]?.GetValue<int>() ?? 0;
            int reasonTok = doc?["usage"]?["completion_tokens_details"]?["reasoning_tokens"]?.GetValue<int>() ?? 0;

            bool modified = !string.Equals(tc.Text.Trim(), reply, StringComparison.Ordinal);
            results.Add((tc, reply, modified, sw.ElapsedMilliseconds, promptTok, compTok, reasonTok));

            Console.WriteLine($"{sw.ElapsedMilliseconds,4}ms | In:{promptTok,3} Out:{compTok,3} (R:{reasonTok,3}) | Mod={modified}");
            await Task.Delay(100);
        }

        var normals = results.Where(x => !x.tc.IsTypo).ToList();
        var typos = results.Where(x => x.tc.IsTypo).ToList();

        int normalPreserved = normals.Count(x => !x.modified);
        int typoModified = typos.Count(x => x.modified);

        Console.WriteLine("\n================================================================================");
        Console.WriteLine("【Prompt v2 適用結果集計】");
        Console.WriteLine($"  正常文の維持率 (過剰修正なし) : {(double)normalPreserved / normals.Count:P1} ({normalPreserved}/{normals.Count})  [v1: 73.3% (11/15)]");
        Console.WriteLine($"  誤字文の修正着手率 (変更検知) : {(double)typoModified / typos.Count:P1} ({typoModified}/{typos.Count})  [v1: 93.3% (14/15)]");
        Console.WriteLine($"  平均レスポンスタイム          : {results.Average(x => x.latency):F1} ms  [v1: 2324.5 ms]");
        Console.WriteLine($"  平均消費トークン数            : {results.Average(x => x.inTok + x.outTok):F1} tokens");
        Console.WriteLine("================================================================================\n");

        Console.WriteLine("■ 正常文 (ID:01〜15) で変更が発生したケースの内訳:");
        var changedNormals = normals.Where(x => x.modified).ToList();
        if (changedNormals.Count == 0)
        {
            Console.WriteLine("  なし！（15件すべて完全一致で維持されました 🎉）");
        }
        else
        {
            foreach (var cn in changedNormals)
            {
                Console.WriteLine($"  ID:{cn.tc.Id:D2} 【{cn.tc.Category}】");
                Console.WriteLine($"    原文: {cn.tc.Text}");
                Console.WriteLine($"    出力: {cn.output}");
            }
        }

        Console.WriteLine("\n■ 誤字文 (ID:16〜30) で修正されなかったケースの内訳:");
        var uncorrectedTypos = typos.Where(x => !x.modified).ToList();
        if (uncorrectedTypos.Count == 0)
        {
            Console.WriteLine("  なし！（15件すべて修正されました 🎉）");
        }
        else
        {
            foreach (var ut in uncorrectedTypos)
            {
                Console.WriteLine($"  ID:{ut.tc.Id:D2} 【{ut.tc.Category}】");
                Console.WriteLine($"    原文: {ut.tc.Text}");
                Console.WriteLine($"    出力: {ut.output}");
            }
        }
    }
}
