"""Reproduce length-specific metrics and offline pipeline estimates from saved responses."""
import difflib
import json
import math
import statistics
import sys
from collections import defaultdict
from pathlib import Path


def mean(values):
    return statistics.mean(values) if values and all(x is not None for x in values) else None


def p95(values):
    return sorted(values)[max(0, math.ceil(len(values) * .95) - 1)] if values else None


def number(value, precision):
    return f'{value:.{precision}f}' if value is not None else 'unknown'


def summarize(rows):
    normal = [r for r in rows if not r['IsTypo']]
    typo = [r for r in rows if r['IsTypo']]
    times = [r['LatencyMs'] for r in rows if not r['Error']]
    return {
        'Attempts': len(rows), 'Errors': sum(bool(r['Error']) for r in rows),
        'Correct': sum(r['Correct'] for r in rows),
        'NormalCorrect': sum(r['Correct'] for r in normal), 'NormalCount': len(normal),
        'TypoCorrect': sum(r['Correct'] for r in typo), 'TypoCount': len(typo),
        'MeanMs': mean(times), 'MedianMs': sorted(times)[math.ceil(len(times) / 2) - 1] if times else None,
        'P95Ms': p95(times), 'MeanCostUsd': mean([r['CostUsd'] for r in rows]),
    }


def changes(expected, actual):
    if actual is None:
        return '(no corrected output)'
    matcher = difflib.SequenceMatcher(a=expected, b=actual, autojunk=False)
    return '; '.join(f'{expected[a:b]!r} -> {actual[c:d]!r} at {a}'
                     for op, a, b, c, d in matcher.get_opcodes() if op != 'equal')


def apply_edits(original, output):
    edits = json.loads(output)
    assert isinstance(edits, list)
    spans = []
    for edit in edits:
        assert isinstance(edit, list) and len(edit) == 2
        before, after = edit
        assert isinstance(before, str) and isinstance(after, str) and before and before != after
        start = original.find(before)
        assert start >= 0 and original.find(before, start + 1) < 0
        spans.append((start, start + len(before), after))
    spans.sort()
    assert all(a[1] <= b[0] for a, b in zip(spans, spans[1:]))
    result = original
    for start, end, after in reversed(spans):
        result = result[:start] + after + result[end:]
    return result


def main(directory):
    manifest = json.loads((directory / 'manifest.json').read_text(encoding='utf-8'))
    completion = json.loads((directory / 'completion.json').read_text(encoding='utf-8'))
    rows = [json.loads(line) for line in (directory / 'requests.jsonl').read_text(encoding='utf-8').splitlines()]
    cases = {c['Id']: c for c in manifest['Cases']}
    variants = [v['Configuration'] for v in manifest['Variants']]
    index = {(r['Variant'], r['Id'], r['Repetition']): r for r in rows}
    expected_keys = {(v['Name'], c, rep) for v in variants for c in cases
                     for rep in range(1, manifest['Repetitions'] + 1)}
    assert set(index) == expected_keys and len(index) == len(rows), 'Missing or duplicate measurements'
    assert completion['Requests'] == len(rows)

    groups = defaultdict(list)
    breakdowns = defaultdict(list)
    failures = []
    for r in rows:
        c = cases[r['Id']]
        assert c['IsTypo'] == r['IsTypo']
        if not r['Result']['Error']:
            if r['Mode'] == 'decision':
                assert r['Predicted'] == (r['Result']['Probability'] >= r['Threshold'])
            else:
                output = apply_edits(c['Text'], r['Result']['Output']) if r['Mode'] == 'edits' else r['Result']['Output']
                assert output == r['Corrected']
                assert r['Predicted'] == (output != c['Text'])
        expected_correct = not r['Result']['Error'] and (
            r['Predicted'] == c['IsTypo'] if r['Mode'] == 'decision'
            else r['Corrected'] in c['ExpectedTexts'])
        assert expected_correct == r['Correct'], f'Score mismatch: {r["Variant"]} {r["Id"]}'
        sample = {'Id': r['Id'], 'Repetition': r['Repetition'], 'IsTypo': c['IsTypo'],
                  'Correct': r['Correct'], 'Error': r['Result']['Error'],
                  'LatencyMs': r['Result']['LatencyMs'], 'CostUsd': r['Result']['CostUsd']}
        groups[(c['TargetLength'], r['Variant'])].append(sample)
        if c['IsTypo']:
            for field in ['ErrorType', 'ErrorPosition', 'ErrorCount']:
                breakdowns[(c['TargetLength'], r['Variant'], field, c[field])].append(sample)
        if not r['Correct']:
            failures.append({'Variant': r['Variant'], 'Id': c['Id'], 'FamilyId': c['FamilyId'],
                             'TargetLength': c['TargetLength'], 'IsTypo': c['IsTypo'],
                             'ErrorType': c['ErrorType'], 'ErrorPosition': c['ErrorPosition'],
                             'Repetition': r['Repetition'], 'Error': r['Result']['Error'],
                             'Probability': r['Result']['Probability'], 'Predicted': r['Predicted'],
                             'ExpectedVsActual': changes(c['ExpectedTexts'][0], r['Corrected']) if r['Mode'] != 'decision' else None})

    metrics = [{'TargetLength': length, 'Variant': variant, **summarize(samples)}
               for (length, variant), samples in sorted(groups.items())]
    category_metrics = [{'TargetLength': length, 'Variant': variant, 'Dimension': field,
                         'Value': value, **summarize(samples)}
                        for (length, variant, field, value), samples in sorted(breakdowns.items())]
    pipelines = []
    decisions = [v['Name'] for v in variants if v['Mode'] == 'decision']
    corrections = [v['Name'] for v in variants if v['Mode'] != 'decision']
    for length in sorted({c['TargetLength'] for c in cases.values()}):
        for decision in [None] + decisions:
            for correction in corrections:
                samples = []
                for c in cases.values():
                    if c['TargetLength'] != length:
                        continue
                    for rep in range(1, manifest['Repetitions'] + 1):
                        d = index[(decision, c['Id'], rep)] if decision else None
                        fix = index[(correction, c['Id'], rep)]
                        called = not d or (not d['Result']['Error'] and d['Predicted'] is True)
                        used = ([d] if d else []) + ([fix] if called else [])
                        error = next((r['Result']['Error'] for r in used if r['Result']['Error']), None)
                        output = fix['Corrected'] if called else c['Text']
                        samples.append({'Id': c['Id'], 'Repetition': rep, 'IsTypo': c['IsTypo'],
                                        'CorrectionCalled': called, 'Error': error,
                                        'Correct': not error and output in c['ExpectedTexts'],
                                        'LatencyMs': sum(r['Result']['LatencyMs'] for r in used),
                                        'CostUsd': sum(r['Result']['CostUsd'] for r in used)
                                        if all(r['Result']['CostUsd'] is not None for r in used) else None})
                pipelines.append({'TargetLength': length, 'Decision': decision, 'Correction': correction,
                                  'CorrectionCalls': sum(s['CorrectionCalled'] for s in samples),
                                  **summarize(samples), 'Samples': samples})

    report = {'Method': 'Paired synthetic documents, two repeats. Pipeline metrics are offline replay at 50% typo prevalence, not end-to-end measurements.',
              'Requests': len(rows), 'UniqueCases': len(cases),
              'TotalCostUsd': sum(r['Result']['CostUsd'] for r in rows) if all(r['Result']['CostUsd'] is not None for r in rows) else None,
              'Metrics': metrics, 'Breakdowns': category_metrics, 'Pipelines': pipelines, 'Failures': failures}
    (directory / 'length-analysis.json').write_text(json.dumps(report, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
    lines = ['# 文字数別の集計', '', '正常・誤字各24文を2回ずつ評価。表の分母48は24種類の文の反復観測です。', '']
    for length in sorted({c['TargetLength'] for c in cases.values()}):
        inputs = [len(c['Text']) for c in cases.values() if c['TargetLength'] == length]
        lines += [f'## {length}文字帯（実測 {min(inputs)}〜{max(inputs)}文字）', '',
                  '| 設定 | 正常正解 | 誤字正解 | エラー | 平均 ms | P95 ms | USD/件 |',
                  '|---|---:|---:|---:|---:|---:|---:|']
        for m in metrics:
            if m['TargetLength'] == length:
                lines.append(f"| {m['Variant']} | {m['NormalCorrect']}/{m['NormalCount']} | {m['TypoCorrect']}/{m['TypoCount']} | {m['Errors']} | {number(m['MeanMs'], 1)} | {number(m['P95Ms'], 1)} | {number(m['MeanCostUsd'], 8)} |")
        lines += ['', '判定と補正を組み合わせた推定値（誤字率50%）。直接補正は実測の集計。', '',
                  '| 判定 | 補正 | 正常維持 | 誤字完全一致 | 平均 ms | USD/件 |',
                  '|---|---|---:|---:|---:|---:|']
        for p in pipelines:
            if p['TargetLength'] == length:
                lines.append(f"| {p['Decision'] or '直接補正'} | {p['Correction']} | {p['NormalCorrect']}/{p['NormalCount']} | {p['TypoCorrect']}/{p['TypoCount']} | {number(p['MeanMs'], 1)} | {number(p['MeanCostUsd'], 8)} |")
        lines += ['']
    (directory / 'length-analysis.md').write_text('\n'.join(lines) + '\n', encoding='utf-8')
    print(json.dumps({'Requests': report['Requests'], 'UniqueCases': len(cases), 'Failures': len(failures), 'TotalCostUsd': report['TotalCostUsd']}, ensure_ascii=False))


if __name__ == '__main__':
    main(Path(sys.argv[1]))
