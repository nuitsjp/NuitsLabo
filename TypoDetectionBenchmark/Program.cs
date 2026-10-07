using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace TypoDetectionBenchmark;

internal static class Program
{
    internal static readonly JsonSerializerOptions Json = new() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

    public static async Task<int> Main(string[] args)
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        try
        {
            if (args.Length == 0 || args[0] == "--help")
            {
                Console.WriteLine("""
                    dotnet run -c Release -- run [--variants name1,name2] [--split development|holdout|all]
                        [--repetitions 3] [--limit N] [--seed 20261008] [--output results/run-name]
                        [--corpus path.json]
                    dotnet run -c Release -- summarize results/run-name
                    dotnet run -c Release -- self-test
                    dotnet run -c Release -- list
                    Default variants: decision-openai-english,correction-luna56-v3-edits-low
                    API calls are sequential, randomized, and interleaved. No retries or fixed delays.
                    Costs are standard-price estimates from usage; no free quota is assumed.
                    """);
                return 0;
            }
            if (args[0] == "self-test")
            {
                Evaluation.SelfTest();
                await ApiClientChecks.RunAsync();
                LoadCorpus(null);
                CorpusSelfTest();
                return 0;
            }
            if (args[0] == "list")
            {
                foreach (var v in Prompts.All) Console.WriteLine($"{v.Name}: {v.Model}, {v.Mode}, effort={v.Effort}, threshold={v.Threshold}");
                return 0;
            }
            if (args[0] == "summarize" && args.Length == 2)
            {
                Summarize(args[1]);
                return 0;
            }
            if (args[0] != "run") throw new ArgumentException("Unknown command; use --help.");
            return await Run(args.Skip(1).ToArray());
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidDataException or IOException or JsonException)
        {
            Console.Error.WriteLine(ex.Message);
            return 1;
        }
    }

    private static async Task<int> Run(string[] args)
    {
        var options = new Dictionary<string, string>();
        var valid = new[] { "--variants", "--split", "--repetitions", "--limit", "--seed", "--output", "--corpus" };
        for (int i = 0; i < args.Length; i += 2)
        {
            if (!valid.Contains(args[i]) || i + 1 == args.Length || !options.TryAdd(args[i], args[i + 1]))
                throw new ArgumentException("Invalid or duplicate option; use --help.");
        }
        int PositiveInt(string key, int defaultValue) => !options.TryGetValue(key, out var value) ? defaultValue
            : int.TryParse(value, out int n) && n > 0 ? n : throw new ArgumentException($"{key} must be a positive integer.");
        int repetitions = PositiveInt("--repetitions", 1);
        int seed = PositiveInt("--seed", 20261008);
        int limit = PositiveInt("--limit", int.MaxValue);
        string split = options.GetValueOrDefault("--split", "development");
        if (split is not ("development" or "holdout" or "all")) throw new ArgumentException("Invalid split.");
        var requested = options.GetValueOrDefault("--variants", "decision-openai-english,correction-luna56-v3-edits-low").Split(',');
        var variants = requested.Select(name =>
            Prompts.All.SingleOrDefault(v => v.Name == name) ?? throw new ArgumentException($"Unknown variant: {name}")).ToArray();
        if (variants.Select(v => v.Name).Distinct().Count() != variants.Length) throw new ArgumentException("Duplicate variants.");
        var corpusPath = options.TryGetValue("--corpus", out var path) ? Path.GetFullPath(path) : null;
        var cases = LoadCorpus(corpusPath).Where(c => split == "all" || c.Split == split).Take(limit).ToArray();
        if (cases.Length == 0) throw new InvalidDataException($"No cases match split '{split}'. Use --split all to include every corpus split.");
        var directory = Path.GetFullPath(options.GetValueOrDefault("--output", $"results/{DateTime.UtcNow:yyyyMMdd-HHmmss-fff}"));
        if (Directory.Exists(directory)) throw new IOException($"Output already exists: {directory}");
        Directory.CreateDirectory(directory);
        var metadata = new
        {
            StartedUtc = DateTimeOffset.UtcNow, Repetitions = repetitions, Seed = seed,
            MaxConcurrency = 1, WarmupRequests = 0, TimeoutSeconds = 30,
            Latency = "SendAsync through complete body read; no retries or artificial delay; first request marked ColdStart",
            Cost = "Standard USD estimate from API usage, pricing checked 2026-10-08; missing usage is unknown, not zero",
            PricingSources = new[] { "https://developers.openai.com/api/docs/guides/decisions", "https://developers.openai.com/api/docs/models/gpt-6-luna", "https://developers.openai.com/api/docs/models/gpt-5.6-luna", "https://typesafe.ai/blog/introducing-system-one-models-and-jev" },
            CorpusPath = corpusPath, CorpusSha256 = Hash(JsonSerializer.Serialize(cases, Json)),
            Variants = variants.Select(v => new { Configuration = v, PromptSha256 = Hash(v.Instructions + JsonSerializer.Serialize(v.Criteria, Json)) }),
            Cases = cases
        };
        File.WriteAllText(Path.Combine(directory, "manifest.json"), JsonSerializer.Serialize(metadata, Json), new UTF8Encoding(false));
        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
        var client = new ApiClient(http);
        using var writer = new StreamWriter(Path.Combine(directory, "requests.jsonl"), false, new UTF8Encoding(false)) { AutoFlush = true };
        var random = new Random(seed);
        var seenProviders = new HashSet<string>();
        var wall = Stopwatch.StartNew();
        int completed = 0, errors = 0;
        Console.WriteLine($"{cases.Length} cases x {variants.Length} variants x {repetitions} repetitions. Output: {directory}");
        for (int repetition = 1; repetition <= repetitions; repetition++)
        {
            var shuffledCases = cases.ToArray();
            random.Shuffle(shuffledCases);
            foreach (var tc in shuffledCases)
            {
                var shuffledVariants = variants.ToArray();
                random.Shuffle(shuffledVariants);
                foreach (var variant in shuffledVariants)
                {
                    bool coldStart = seenProviders.Add(variant.Provider);
                    var result = await client.EvaluateAsync(variant, tc.Text);
                    string? corrected = null;
                    bool? predicted = null;
                    if (result.Error == null)
                    {
                        try
                        {
                            if (variant.Mode == "decision") predicted = result.Probability!.Value >= variant.Threshold;
                            else
                            {
                                corrected = variant.Mode == "edits" ? Evaluation.ApplyEdits(tc.Text, result.Output!) : result.Output!;
                                predicted = !string.Equals(tc.Text, corrected, StringComparison.Ordinal);
                            }
                        }
                        catch (Exception ex) when (ex is InvalidDataException or JsonException)
                        {
                            result = result with { Error = $"Invalid correction: {ex.Message}" };
                        }
                    }
                    bool correct = result.Error == null && (variant.Mode == "decision"
                        ? predicted == tc.IsTypo : Evaluation.IsCorrect(tc, corrected!));
                    var row = new Measurement(variant.Name, variant.Mode, tc.Id, tc.Split, tc.IsTypo, repetition, coldStart,
                        variant.Threshold, predicted, correct, corrected, result);
                    writer.WriteLine(JsonSerializer.Serialize(row, Json));
                    completed++;
                    if (result.Error != null) errors++;
                    Console.WriteLine($"[{completed}/{cases.Length * variants.Length * repetitions}] {variant.Name} ID={tc.Id} {result.LatencyMs:F0}ms {(correct ? "OK" : "MISS")} {result.Error}");
                    if (result.HttpStatus is 400 or 401 or 402 or 403 or 404 or 422 or 429)
                    {
                        Console.Error.WriteLine("Aborting run after API rejection; partial results retained.");
                        writer.Dispose();
                        Summarize(directory);
                        return 1;
                    }
                }
            }
        }
        wall.Stop();
        writer.Dispose();
        File.WriteAllText(Path.Combine(directory, "completion.json"), JsonSerializer.Serialize(new { CompletedUtc = DateTimeOffset.UtcNow, Requests = completed, Errors = errors, WallMs = wall.Elapsed.TotalMilliseconds }, Json));
        Summarize(directory);
        return errors == 0 ? 0 : 1;
    }

    private static string Hash(string value) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value)));

    private static BenchmarkCase[] LoadCorpus(string? path)
    {
        var cases = path == null ? TestCorpus.All
            : JsonSerializer.Deserialize<BenchmarkCase[]>(File.ReadAllText(path, Encoding.UTF8),
                new JsonSerializerOptions(Json) { RespectRequiredConstructorParameters = true })
                ?? throw new InvalidDataException("Corpus must be a JSON array.");
        ValidateCorpus(cases);
        return cases;
    }

    private static void ValidateCorpus(BenchmarkCase[] cases)
    {
        if (cases.Length == 0) throw new InvalidDataException("Corpus must contain at least one case.");
        if (cases.Any(c => c == null)) throw new InvalidDataException("Corpus cases must not be null.");
        if (cases.Select(c => c.Id).Distinct().Count() != cases.Length) throw new InvalidDataException("Corpus IDs are not unique.");
        foreach (var tc in cases)
        {
            if (string.IsNullOrWhiteSpace(tc.Split) || string.IsNullOrWhiteSpace(tc.Category) || string.IsNullOrEmpty(tc.Text))
                throw new InvalidDataException($"Missing split, category or text for case {tc.Id}.");
            if (tc.TargetLength is <= 0 || tc.ErrorCount is < 0)
                throw new InvalidDataException($"Invalid length or error count for case {tc.Id}.");
            if (tc.ExpectedTexts == null || tc.ExpectedTexts.Length == 0 || tc.ExpectedTexts.Any(string.IsNullOrEmpty)
                || (tc.IsTypo ? tc.ExpectedTexts.Contains(tc.Text, StringComparer.Ordinal) : tc.ExpectedTexts.Length != 1 || tc.ExpectedTexts[0] != tc.Text))
                throw new InvalidDataException($"Invalid expected text for case {tc.Id}.");
        }
        if (cases.Select(c => c.Text).Distinct(StringComparer.Ordinal).Count() != cases.Length)
            throw new InvalidDataException("Corpus texts are not unique.");
        Console.WriteLine($"Corpus validated: {cases.Length} cases, {cases.Min(c => c.Text.Length)}–{cases.Max(c => c.Text.Length)} characters.");
    }

    private static void CorpusSelfTest()
    {
        var path = Path.GetTempFileName();
        int passed = 0;
        BenchmarkCase[] Read(string json)
        {
            File.WriteAllText(path, json, new UTF8Encoding(false));
            return LoadCorpus(path);
        }
        void Reject(string json, string name)
        {
            try { Read(json); }
            catch (Exception ex) when (ex is InvalidDataException or JsonException) { passed++; return; }
            throw new InvalidOperationException($"Corpus self-test failed: {name} was accepted.");
        }
        try
        {
            var normal = new BenchmarkCase(1, "length", false, "normal", "正常な文。", ["正常な文。"], 100, 1, "none", "none", 0);
            var typo = new BenchmarkCase(2, "length", true, "typo", "正常なな文。", ["正常な文。"], 100, 1, "duplicate", "middle", 1);
            var loaded = Read(JsonSerializer.Serialize(new[] { normal, typo }, Json));
            if (loaded.Length != 2 || loaded[0].Text != normal.Text || loaded[1].ExpectedTexts[0] != typo.ExpectedTexts[0]
                || loaded[1].TargetLength != 100 || loaded[1].FamilyId != 1 || loaded[1].ErrorType != "duplicate"
                || loaded[1].ErrorPosition != "middle" || loaded[1].ErrorCount != 1 || loaded[1].Split != "length")
                throw new InvalidOperationException("Corpus self-test failed: JSON values were not preserved.");
            passed++;
            var withoutMetadata = Read("""[{"Id":1,"Split":"development","IsTypo":false,"Category":"normal","Text":"abc","ExpectedTexts":["abc"]}]""");
            if (withoutMetadata[0].TargetLength != null || withoutMetadata[0].FamilyId != null || withoutMetadata[0].ErrorType != null
                || withoutMetadata[0].ErrorPosition != null || withoutMetadata[0].ErrorCount != null)
                throw new InvalidOperationException("Corpus self-test failed: optional metadata defaults.");
            passed++;
            Reject("[", "malformed JSON");
            Reject("{}", "object root");
            Reject("null", "null root");
            Reject("[]", "empty corpus");
            Reject("[null]", "null case");
            Reject("""[{"Id":1,"Split":"development","Category":"normal","Text":"abc","ExpectedTexts":["abc"]}]""", "missing required field");
            Reject(JsonSerializer.Serialize(new[] { normal, typo with { Id = normal.Id } }, Json), "duplicate IDs");
            Reject(JsonSerializer.Serialize(new[] { normal, normal with { Id = 2 } }, Json), "duplicate text");
            Reject(JsonSerializer.Serialize(new[] { normal with { Text = "" } }, Json), "empty text");
            Reject(JsonSerializer.Serialize(new[] { normal with { ExpectedTexts = null! } }, Json), "null expected texts");
            Reject(JsonSerializer.Serialize(new[] { normal with { ExpectedTexts = [] } }, Json), "empty expected texts");
            Reject(JsonSerializer.Serialize(new[] { normal with { ExpectedTexts = ["別の文。"] } }, Json), "normal text changed by gold");
            Reject(JsonSerializer.Serialize(new[] { typo with { ExpectedTexts = [typo.Text] } }, Json), "typo unchanged by gold");
            Reject(JsonSerializer.Serialize(new[] { normal with { TargetLength = 0 } }, Json), "nonpositive target length");
            Reject(JsonSerializer.Serialize(new[] { typo with { ErrorCount = -1 } }, Json), "negative error count");
        }
        finally { File.Delete(path); }
        Console.WriteLine($"Corpus self-test: {passed} passed.");
    }

    internal static void Summarize(string directory)
    {
        using var manifest = JsonDocument.Parse(File.ReadAllText(Path.Combine(directory, "manifest.json"), Encoding.UTF8));
        var cases = JsonSerializer.Deserialize<BenchmarkCase[]>(manifest.RootElement.GetProperty("Cases"), Json)!;
        var casesById = cases.ToDictionary(c => c.Id);
        var rows = File.ReadLines(Path.Combine(directory, "requests.jsonl")).Select(line => JsonSerializer.Deserialize<Measurement>(line, Json)!).ToArray();
        var summaries = rows.GroupBy(r => new { r.Variant, r.Split, casesById[r.Id].TargetLength }).Select(g =>
        {
            var all = g.ToArray();
            var successful = all.Where(r => r.Result.Error == null).ToArray();
            var normal = all.Where(r => !r.IsTypo).ToArray();
            var typo = all.Where(r => r.IsTypo).ToArray();
            var latency = successful.Select(r => r.Result.LatencyMs).ToArray();
            int tp = successful.Count(r => r.IsTypo && r.Predicted == true), fp = successful.Count(r => !r.IsTypo && r.Predicted == true);
            int tn = successful.Count(r => !r.IsTypo && r.Predicted == false), fn = successful.Count(r => r.IsTypo && r.Predicted == false);
            return new
            {
                g.Key.Variant, g.Key.Split, g.Key.TargetLength, Attempts = all.Length, Successes = successful.Length, Errors = all.Length - successful.Length,
                Correct = all.Count(r => r.Correct), Accuracy = (double)all.Count(r => r.Correct) / all.Length,
                NormalCorrect = normal.Count(r => r.Correct), NormalCount = normal.Length,
                TypoCorrect = typo.Count(r => r.Correct), TypoCount = typo.Length,
                TP = g.First().Mode == "decision" ? (int?)tp : null,
                FP = g.First().Mode == "decision" ? (int?)fp : null,
                TN = g.First().Mode == "decision" ? (int?)tn : null,
                FN = g.First().Mode == "decision" ? (int?)fn : null,
                Recall = g.First().Mode == "decision" && tp + fn > 0 ? (double?)tp / (tp + fn) : null,
                Precision = g.First().Mode == "decision" && tp + fp > 0 ? (double?)tp / (tp + fp) : null,
                F1 = g.First().Mode == "decision" && 2 * tp + fp + fn > 0 ? (double?)(2 * tp) / (2 * tp + fp + fn) : null,
                MeanMs = latency.Length > 0 ? (double?)latency.Average() : null,
                MedianMs = latency.Length > 0 ? (double?)Evaluation.Percentile(latency, 0.5) : null,
                P95Ms = latency.Length > 0 ? (double?)Evaluation.Percentile(latency, 0.95) : null,
                WarmMeanMs = successful.Where(r => !r.ColdStart).Select(r => (double?)r.Result.LatencyMs).Average(),
                MeanInputTokens = all.All(r => r.Result.InputTokens != null) ? all.Average(r => (double?)r.Result.InputTokens) : null,
                MeanOutputTokens = all.All(r => r.Result.OutputTokens != null) ? all.Average(r => (double?)r.Result.OutputTokens) : null,
                MeanReasoningTokens = all.All(r => r.Result.ReasoningTokens != null) ? all.Average(r => (double?)r.Result.ReasoningTokens) : null,
                MeanCostUsd = all.All(r => r.Result.CostUsd != null) ? all.Average(r => r.Result.CostUsd) : null,
                TotalCostUsd = all.All(r => r.Result.CostUsd != null) ? all.Sum(r => r.Result.CostUsd) : null
            };
        }).ToArray();
        File.WriteAllText(Path.Combine(directory, "summary.json"), JsonSerializer.Serialize(summaries, Json), new UTF8Encoding(false));
        var markdown = new StringBuilder("| Variant | Split | Target length | Correct / attempts | Errors | Normal | Typo | Mean ms | P95 ms | USD / request |\n|---|---|---:|---:|---:|---:|---:|---:|---:|---:|\n");
        foreach (var s in summaries)
            markdown.AppendLine($"| {s.Variant} | {s.Split} | {s.TargetLength?.ToString() ?? "-"} | {s.Correct}/{s.Attempts} | {s.Errors} | {s.NormalCorrect}/{s.NormalCount} | {s.TypoCorrect}/{s.TypoCount} | {s.MeanMs:F1} | {s.P95Ms:F1} | {(s.MeanCostUsd.HasValue ? s.MeanCostUsd.Value.ToString("F8") : "unknown")} |");
        File.WriteAllText(Path.Combine(directory, "summary.md"), markdown.ToString(), new UTF8Encoding(false));
        Console.WriteLine(markdown);
    }
}

internal record Measurement(string Variant, string Mode, int Id, string Split, bool IsTypo, int Repetition,
    bool ColdStart, double Threshold, bool? Predicted, bool Correct, string? Corrected, ApiResult Result);
