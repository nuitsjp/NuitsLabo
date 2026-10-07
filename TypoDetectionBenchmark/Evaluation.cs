using System.Text;
using System.Text.Json;

namespace TypoDetectionBenchmark;

internal static class Evaluation
{
    public static string ApplyEdits(string original, string json)
    {
        using var document = JsonDocument.Parse(json);
        if (document.RootElement.ValueKind != JsonValueKind.Array)
            throw new InvalidDataException("Edits must be a JSON array.");

        var edits = new List<(int Start, int Length, string After)>();
        foreach (var item in document.RootElement.EnumerateArray())
        {
            if (item.ValueKind != JsonValueKind.Array || item.GetArrayLength() != 2 ||
                item[0].ValueKind != JsonValueKind.String || item[1].ValueKind != JsonValueKind.String)
                throw new InvalidDataException("Each edit must contain exactly two strings: [before, after].");

            string before, after;
            try
            {
                before = item[0].GetString()!;
                after = item[1].GetString()!;
            }
            catch (InvalidOperationException ex)
            {
                throw new JsonException("Edit strings must contain valid UTF-16.", ex);
            }
            if (before.Length == 0 || string.Equals(before, after, StringComparison.Ordinal))
                throw new InvalidDataException("An edit must replace a nonempty string with a different string.");

            var start = original.IndexOf(before, StringComparison.Ordinal);
            if (start < 0)
                throw new InvalidDataException("The edit's before string was not found in the original text.");
            // Advance one character so overlapping occurrences also count as ambiguous.
            if (original.IndexOf(before, start + 1, StringComparison.Ordinal) >= 0)
                throw new InvalidDataException("The edit's before string must occur exactly once in the original text.");

            edits.Add((start, before.Length, after));
        }

        edits.Sort((left, right) => left.Start.CompareTo(right.Start));
        for (var i = 1; i < edits.Count; i++)
        {
            if (edits[i].Start < edits[i - 1].Start + edits[i - 1].Length)
                throw new InvalidDataException("Edit ranges in the original text must not overlap.");
        }

        if (edits.Count == 0)
            return original;

        var result = new StringBuilder(original);
        for (var i = edits.Count - 1; i >= 0; i--)
        {
            var edit = edits[i];
            result.Remove(edit.Start, edit.Length).Insert(edit.Start, edit.After);
        }
        return result.ToString();
    }

    public static bool IsCorrect(BenchmarkCase tc, string output) =>
        tc.ExpectedTexts.Any(expected => string.Equals(expected, output, StringComparison.Ordinal));

    public static double Percentile(IEnumerable<double> samples, double p)
    {
        if (double.IsNaN(p) || p < 0 || p > 1)
            throw new ArgumentOutOfRangeException(nameof(p), "The percentile must be between 0 and 1.");

        var sorted = samples.Order().ToArray();
        if (sorted.Length == 0)
            throw new InvalidOperationException("A percentile requires at least one sample.");

        var rank = Math.Max(1, (int)Math.Ceiling(sorted.Length * p));
        return sorted[rank - 1];
    }

    public static void SelfTest()
    {
        var passed = 0;

        void Check(bool condition, string name)
        {
            if (!condition)
                throw new InvalidOperationException($"Self-test failed: {name}");
            passed++;
        }

        void RejectEdit(string original, string json, string name)
        {
            try
            {
                ApplyEdits(original, json);
            }
            catch (Exception ex) when (ex is InvalidDataException or JsonException)
            {
                passed++;
                return;
            }
            throw new InvalidOperationException($"Self-test failed: {name} was accepted.");
        }

        Check(ApplyEdits("誤字と脱時", "[[\"脱時\",\"脱字\"],[\"誤字\",\"漢字\"]]") == "漢字と脱字",
            "multiple edits in arbitrary order");
        Check(ApplyEdits("a-b", "[[\"a\",\"long\"],[\"b\",\"終\"]]") == "long-終",
            "length-changing edits use original positions");
        Check(ApplyEdits("a b", "[[\"a\",\"b\"],[\"b\",\"c\"]]") == "b c",
            "replacement output is not edited again");
        Check(ApplyEdits("a", "[[\"a\",\"\"]]") == "", "deletion");
        Check(ApplyEdits("ab", "[[\"a\",\"x\"],[\"b\",\"y\"]]") == "xy", "adjacent edits");

        const string untouched = " \t😀 e\u0301 が　\r\n";
        Check(ApplyEdits(untouched, "[]") == untouched, "unchanged whitespace and Unicode");
        Check(ApplyEdits(untouched, "[[\"が\",\"か\"]]") == " \t😀 e\u0301 か　\r\n",
            "edits preserve surrounding whitespace and Unicode");
        Check(ApplyEdits("😀", "[[\"\\uD83D\\uDE00\",\"笑\"]]") == "笑",
            "valid escaped surrogate pair");

        RejectEdit("a", "[", "invalid JSON");
        RejectEdit("a", "{}", "object root");
        RejectEdit("a", "null", "null root");
        RejectEdit("a", "[\"a\",\"b\"]", "flat array");
        RejectEdit("a", "[[\"a\"]]", "missing replacement");
        RejectEdit("a", "[[\"a\",\"b\",\"c\"]]", "extra member");
        RejectEdit("a", "[[\"a\",null]]", "null replacement");
        RejectEdit("a", "[[1,\"b\"]]", "nonstring source");
        RejectEdit("a", "[[\"a\",1]]", "nonstring replacement");
        RejectEdit("a", "[[\"\\uD800\",\"b\"]]", "unpaired high surrogate in source");
        RejectEdit("a", "[[\"a\",\"\\uD800\"]]", "unpaired high surrogate in replacement");
        RejectEdit("a", "[[\"a\",\"\\uDC00\"]]", "unpaired low surrogate in replacement");
        RejectEdit("a", "[[\"a\",\"\\uD800x\"]]", "high surrogate followed by a non-surrogate");
        RejectEdit("a", "[[\"\",\"b\"]]", "empty source");
        RejectEdit("a", "[[\"a\",\"a\"]]", "unchanged edit");
        RejectEdit("a", "[[\"b\",\"c\"]]", "source absent from original");
        RejectEdit("a a", "[[\"a\",\"b\"]]", "ambiguous source");
        RejectEdit("aaa", "[[\"aa\",\"b\"]]", "overlapping source occurrences");
        RejectEdit("abc", "[[\"ab\",\"x\"],[\"bc\",\"y\"]]", "overlapping edit ranges");
        RejectEdit("abc", "[[\"abc\",\"x\"],[\"b\",\"y\"]]", "nested edit ranges");
        RejectEdit("a", "[[\"a\",\"b\"],[\"a\",\"c\"]]", "duplicate edit ranges");
        RejectEdit("a", "[[\"a\",\"b\"],[\"b\",\"c\"]]", "source introduced by an earlier edit");

        var typo = new BenchmarkCase(1, "self-test", true, "spelling", "誤時", ["誤字", "ごじ"]);
        Check(IsCorrect(typo, "誤字"), "exact expected correction");
        Check(IsCorrect(typo, "ごじ"), "alternative expected correction");
        Check(!IsCorrect(typo, "別の文"), "arbitrary change is not a correction");
        Check(!IsCorrect(typo, typo.Text), "missed typo");
        Check(!IsCorrect(typo, " 誤字\n"), "scoring does not trim");
        var normal = new BenchmarkCase(2, "self-test", false, "normal", untouched, [untouched]);
        Check(IsCorrect(normal, untouched), "unchanged normal text");
        Check(!IsCorrect(normal, untouched.Trim()), "normal text whitespace must match");
        var ordinal = new BenchmarkCase(3, "self-test", false, "Unicode", "e\u0301A", ["e\u0301A"]);
        Check(!IsCorrect(ordinal, "éA"), "scoring does not normalize Unicode");
        Check(!IsCorrect(ordinal, "e\u0301a"), "scoring is case-sensitive");

        Check(Percentile([4, 1, 3, 2], 0.5) == 2, "nearest-rank median");
        Check(Percentile([3, 1, 2], 0.5) == 2, "odd sample median");
        Check(Percentile([4, 1, 3, 2], 0.95) == 4, "nearest-rank p95");
        Check(Percentile([4, 1, 3, 2], 0) == 1, "minimum percentile");
        Check(Percentile([4, 1, 3, 2], 1) == 4, "maximum percentile");
        Check(Percentile([7], 0.5) == 7, "single sample percentile");
        try
        {
            Percentile([], 0.5);
            throw new InvalidOperationException("Self-test failed: empty samples were accepted.");
        }
        catch (InvalidOperationException ex) when (ex.Message == "A percentile requires at least one sample.")
        {
            passed++;
        }

        Console.WriteLine($"Evaluation self-test: {passed} passed.");
    }
}
