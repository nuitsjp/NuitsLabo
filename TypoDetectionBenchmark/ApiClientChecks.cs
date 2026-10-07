using System.Net;
using System.Text.Json;

namespace TypoDetectionBenchmark;

internal static class ApiClientChecks
{
    public static async Task RunAsync()
    {
        var originalOpenAiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        var originalTypeSafeKey = Environment.GetEnvironmentVariable("TYPESAFE_API_KEY");
        try
        {
            Environment.SetEnvironmentVariable("OPENAI_API_KEY", "test-credential");
            Environment.SetEnvironmentVariable("TYPESAFE_API_KEY", "test-typesafe");
            var passed = 0;
            void Check(bool condition, string name)
            {
                if (!condition) throw new InvalidOperationException($"API self-test failed: {name}");
                passed++;
            }

            var decisionVariant = new Variant("test", "openai-decisions", "gpt-6-luna", "decision", "Check typos");
            var typeSafeVariant = decisionVariant with { Provider = "typesafe" };
            var chatVariant = new Variant("test", "openai-chat", "gpt-6-luna", "edits", "Return JSON");

            var (decision, _) = await Run(decisionVariant,
                """{"answers":[{"type":"predicate","name":"has_typo","probability":0.8}],"usage":{"input_tokens":200,"output_tokens":0,"input_tokens_details":{"cached_tokens":0},"output_tokens_details":{"reasoning_tokens":0}}}""");
            Check(decision.Error is null && decision.Probability == 0.8 && decision.CostUsd == 0.00002m,
                "Decision usage/cost");

            var (typeSafe, _) = await Run(typeSafeVariant,
                """{"answers":{"has_typo":{"type":"noul","noul":0.2}},"usage":{"input_tokens":100,"output_tokens":1}}""");
            Check(typeSafe.Error is null && typeSafe.Probability == 0.2 && typeSafe.CostUsd == 0.0000042m
                && typeSafe.CachedInputTokens is null, "TypeSafe usage/cost");

            var (chat, chatRequest) = await Run(chatVariant,
                """{"choices":[{"message":{"content":"[]","refusal":null},"finish_reason":"stop"}],"usage":{"prompt_tokens":100,"completion_tokens":10,"prompt_tokens_details":{"cached_tokens":20},"completion_tokens_details":{"reasoning_tokens":3}}}""");
            Check(chat.Error is null && chat.Output == "[]" && chat.CostUsd == 0.0000132m
                && chat.ReasoningTokens == 3, "Chat usage/cost/content");
            using var sent = JsonDocument.Parse(chatRequest);
            Check(!sent.RootElement.GetProperty("store").GetBoolean()
                && sent.RootElement.GetProperty("max_completion_tokens").GetInt32() == 2048
                && sent.RootElement.GetProperty("reasoning_effort").GetString() == "none",
                "Chat request controls");

            var (missingUsage, _) = await Run(chatVariant,
                """{"choices":[{"message":{"content":"[]"},"finish_reason":"stop"}]}""");
            Check(missingUsage.Error is null && missingUsage.InputTokens is null
                && missingUsage.CachedInputTokens is null && missingUsage.OutputTokens is null
                && missingUsage.CostUsd is null, "Missing usage remains null");

            var (httpError, _) = await Run(decisionVariant,
                """{"error":"invalid test-credential"}""", HttpStatusCode.Unauthorized);
            Check(httpError.Error == "HTTP 401." && httpError.Probability is null
                && httpError.RawResponse is not null && !httpError.RawResponse.Contains("test-credential")
                && httpError.RawResponse.Contains("[REDACTED]"), "HTTP failure/redaction");

            var (invalidJson, _) = await Run(decisionVariant, "not JSON");
            Check(invalidJson.Error is not null && invalidJson.Probability is null
                && invalidJson.RawResponse == "not JSON", "Invalid JSON");

            const string invalidUnicodeJson = """{"choices":[{"message":{"content":"\uD800"},"finish_reason":"stop"}]}""";
            var (invalidUnicode, _) = await Run(chatVariant, invalidUnicodeJson);
            Check(invalidUnicode.Error == "Invalid JSON response or token usage."
                && invalidUnicode.Output is null && invalidUnicode.RawResponse == invalidUnicodeJson,
                "Invalid Unicode retained as an API error");

            var (missingProbability, _) = await Run(decisionVariant,
                """{"answers":[{"type":"predicate","name":"has_typo"}]}""");
            Check(missingProbability.Error is not null && missingProbability.Probability is null,
                "Missing probability");

            var (invalidProbability, _) = await Run(decisionVariant,
                """{"answers":[{"type":"predicate","name":"has_typo","probability":2}]}""");
            Check(invalidProbability.Error is not null && invalidProbability.Probability is null,
                "Probability range");

            var (refusal, _) = await Run(decisionVariant,
                """{"answers":[{"type":"refusal","name":"has_typo"}]}""");
            Check(refusal.Error == "Model refused the request." && refusal.Probability is null,
                "Decision refusal");

            var (chatRefusal, _) = await Run(chatVariant,
                """{"choices":[{"message":{"content":null,"refusal":"Cannot comply"},"finish_reason":"stop"}]}""");
            Check(chatRefusal.Error == "Model refused the request.", "Chat refusal");

            var (truncated, _) = await Run(chatVariant,
                """{"choices":[{"message":{"content":"partial"},"finish_reason":"length"}]}""");
            Check(truncated.Error == "Completion truncated at token limit." && truncated.Output == "partial",
                "Truncated content retained");

            var (badUsage, _) = await Run(chatVariant,
                """{"choices":[{"message":{"content":"[]"},"finish_reason":"stop"}],"usage":{"prompt_tokens":-1}}""");
            Check(badUsage.Error is not null && badUsage.CostUsd is null, "Invalid usage");

            var (noCacheCount, _) = await Run(chatVariant,
                """{"choices":[{"message":{"content":"[]"},"finish_reason":"stop"}],"usage":{"prompt_tokens":100,"completion_tokens":10}}""");
            Check(noCacheCount.Error is null && noCacheCount.CostUsd is null
                && noCacheCount.CachedInputTokens is null, "Missing cache count not invented");

            Console.WriteLine($"API self-test: {passed} assertions passed; no network requests.");
        }
        finally
        {
            Environment.SetEnvironmentVariable("OPENAI_API_KEY", originalOpenAiKey);
            Environment.SetEnvironmentVariable("TYPESAFE_API_KEY", originalTypeSafeKey);
        }
    }

    private static async Task<(ApiResult Result, string RequestBody)> Run(
        Variant variant, string json, HttpStatusCode code = HttpStatusCode.OK)
    {
        using var handler = new FakeHandler(json, code);
        using var http = new HttpClient(handler);
        var result = await new ApiClient(http).EvaluateAsync(variant, "入力");
        return (result, handler.RequestBody!);
    }

    private sealed class FakeHandler(string json, HttpStatusCode code) : HttpMessageHandler
    {
        public string? RequestBody { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestBody = await request.Content!.ReadAsStringAsync(cancellationToken);
            return new HttpResponseMessage(code) { Content = new StringContent(json) };
        }
    }
}
