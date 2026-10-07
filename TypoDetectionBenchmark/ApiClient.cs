using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace TypoDetectionBenchmark;

internal record Variant(string Name, string Provider, string Model, string Mode,
    string Instructions, string Effort = "none", double Threshold = 0.5, object? Criteria = null);

internal record ApiResult(double? Probability, string? Output, int? InputTokens,
    int? CachedInputTokens, int? OutputTokens, int? ReasoningTokens, decimal? CostUsd,
    double LatencyMs, int? HttpStatus, string? Error, string? RawResponse);

internal sealed class ApiClient(HttpClient http)
{
    public async Task<ApiResult> EvaluateAsync(Variant variant, string text)
    {
        double? probability = null;
        string? output = null;
        int? inputTokens = null, cachedTokens = null, outputTokens = null, reasoningTokens = null;
        decimal? cost = null;
        int? status = null;
        string? raw = null;
        var timer = new Stopwatch();

        ApiResult Result(string? error = null) => new(probability, output, inputTokens,
            cachedTokens, outputTokens, reasoningTokens, cost, timer.Elapsed.TotalMilliseconds,
            status, error, raw);

        var keyName = variant.Provider switch
        {
            "openai-decisions" or "openai-chat" => "OPENAI_API_KEY",
            "typesafe" => "TYPESAFE_API_KEY",
            _ => throw new ArgumentException("Unsupported provider.", nameof(variant))
        };
        var apiKey = Environment.GetEnvironmentVariable(keyName)
            ?? Environment.GetEnvironmentVariable(keyName, EnvironmentVariableTarget.User);
        if (string.IsNullOrWhiteSpace(apiKey)) return Result($"{keyName} not found.");

        var endpoint = variant.Provider switch
        {
            "openai-decisions" => "https://api.openai.com/v1/decisions",
            "typesafe" => "https://api.typesafe.ai/v1/systemone",
            _ => "https://api.openai.com/v1/chat/completions"
        };
        object body = variant.Provider switch
        {
            "openai-decisions" => new
            {
                model = variant.Model,
                input = text,
                questions = new[] { new { type = "predicate", name = "has_typo", instructions = variant.Instructions } }
            },
            "typesafe" => new
            {
                model = variant.Model,
                state = text,
                questions = new Dictionary<string, object>
                {
                    ["has_typo"] = new { type = "noul", instructions = variant.Instructions, criteria = variant.Criteria }
                }
            },
            _ => new
            {
                model = variant.Model,
                messages = new[]
                {
                    new { role = "system", content = variant.Instructions },
                    new { role = "user", content = text }
                },
                reasoning_effort = variant.Effort,
                max_completion_tokens = 2048,
                store = false
            }
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        try
        {
            timer.Start();
            using var response = await http.SendAsync(request);
            status = (int)response.StatusCode;
            raw = await response.Content.ReadAsStringAsync();
            timer.Stop();
            // Authentication errors can echo credentials; retain the response with that value redacted.
            raw = raw.Replace(apiKey, "[REDACTED]", StringComparison.Ordinal);

            using var document = JsonDocument.Parse(raw);
            var root = document.RootElement;
            var usage = Property(root, "usage");
            if (variant.Provider == "openai-chat")
            {
                inputTokens = TokenCount(Property(usage, "prompt_tokens"));
                outputTokens = TokenCount(Property(usage, "completion_tokens"));
                cachedTokens = TokenCount(Property(Property(usage, "prompt_tokens_details"), "cached_tokens"));
                reasoningTokens = TokenCount(Property(Property(usage, "completion_tokens_details"), "reasoning_tokens"));
            }
            else
            {
                inputTokens = TokenCount(Property(usage, "input_tokens"));
                outputTokens = TokenCount(Property(usage, "output_tokens"));
                cachedTokens = TokenCount(Property(Property(usage, "input_tokens_details"), "cached_tokens"));
                reasoningTokens = TokenCount(Property(Property(usage, "output_tokens_details"), "reasoning_tokens"));
            }
            cost = CalculateCost(variant, inputTokens, cachedTokens, outputTokens);
            if (!response.IsSuccessStatusCode) return Result($"HTTP {status}.");
            if (Property(root, "error").ValueKind is not (JsonValueKind.Undefined or JsonValueKind.Null))
                return Result("API returned an error object.");

            if (variant.Provider == "openai-chat")
            {
                var choices = Property(root, "choices");
                if (choices.ValueKind != JsonValueKind.Array || choices.GetArrayLength() != 1)
                    return Result("Expected exactly one completion choice.");
                var choice = choices[0];
                var message = Property(choice, "message");
                output = StringValue(Property(message, "content"));
                if (Property(message, "refusal").ValueKind is not (JsonValueKind.Undefined or JsonValueKind.Null))
                    return Result("Model refused the request.");
                var finishReason = StringValue(Property(choice, "finish_reason"));
                if (finishReason != "stop")
                    return Result(finishReason == "length" ? "Completion truncated at token limit."
                        : finishReason == "content_filter" ? "Completion blocked by content filter."
                        : "Completion did not finish with stop.");
                if (string.IsNullOrWhiteSpace(output)) return Result("Missing completion content.");
            }
            else
            {
                var answers = Property(root, "answers");
                JsonElement answer;
                string probabilityField;
                if (variant.Provider == "openai-decisions")
                {
                    if (answers.ValueKind != JsonValueKind.Array || answers.GetArrayLength() != 1)
                        return Result("Expected exactly one decision answer.");
                    answer = answers[0];
                    if (StringValue(Property(answer, "type")) == "refusal")
                        return Result("Model refused the request.");
                    if (StringValue(Property(answer, "name")) != "has_typo"
                        || StringValue(Property(answer, "type")) != "predicate")
                        return Result("Missing has_typo predicate answer.");
                    probabilityField = "probability";
                }
                else
                {
                    answer = Property(answers, "has_typo");
                    if (StringValue(Property(answer, "type")) != "noul")
                        return Result("Missing has_typo noul answer.");
                    probabilityField = "noul";
                }
                var value = Property(answer, probabilityField);
                if (value.ValueKind != JsonValueKind.Number || !value.TryGetDouble(out var parsed)
                    || !double.IsFinite(parsed) || parsed is < 0 or > 1)
                    return Result("Missing or invalid probability.");
                probability = parsed;
            }
            return Result();
        }
        catch (JsonException)
        {
            timer.Stop();
            return Result(status is < 200 or >= 300 ? $"HTTP {status}; invalid JSON response." : "Invalid JSON response or token usage.");
        }
        catch (OperationCanceledException)
        {
            timer.Stop();
            return Result("Request timed out or was canceled.");
        }
        catch (HttpRequestException)
        {
            timer.Stop();
            return Result("HTTP transport failed.");
        }
    }

    private static JsonElement Property(JsonElement element, string name) =>
        element.ValueKind == JsonValueKind.Object && element.TryGetProperty(name, out var value) ? value : default;

    private static string? StringValue(JsonElement element)
    {
        if (element.ValueKind != JsonValueKind.String) return null;
        try
        {
            return element.GetString();
        }
        catch (InvalidOperationException exception)
        {
            throw new JsonException("Invalid Unicode in response string.", exception);
        }
    }

    private static int? TokenCount(JsonElement element)
    {
        if (element.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null) return null;
        if (element.ValueKind != JsonValueKind.Number || !element.TryGetInt32(out var value) || value < 0)
            throw new JsonException("Invalid token usage.");
        return value;
    }

    private static decimal? CalculateCost(Variant variant, int? input, int? cached, int? output)
    {
        if (input is null) return null;
        // Standard list prices, USD per 1M tokens, checked 2026-10-08.
        // https://typesafe.ai/blog/introducing-system-one-models-and-jev
        if (variant.Provider == "typesafe") return input.Value * 0.042m / 1_000_000m;
        // Decisions bills input tokens only; output is free.
        if (variant.Provider == "openai-decisions") return input.Value * 0.10m / 1_000_000m;
        if (cached is null || output is null) return null;
        if (cached > input) throw new JsonException("Cached token count exceeds input token count.");
        // https://developers.openai.com/api/docs/models/gpt-6-luna
        // https://developers.openai.com/api/docs/models/gpt-5.6-luna
        return variant.Model switch
        {
            "gpt-6-luna" => ((input.Value - cached.Value) * 0.10m + cached.Value * 0.01m + output.Value * 0.50m) / 1_000_000m,
            "gpt-5.6-luna" => ((input.Value - cached.Value) * 0.20m + cached.Value * 0.02m + output.Value * 1.20m) / 1_000_000m,
            _ => null
        };
    }
}
