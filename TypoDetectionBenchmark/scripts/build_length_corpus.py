"""Validate the authored pairs and produce the benchmark's frozen JSON corpus."""
import hashlib
import json
from collections import Counter
from pathlib import Path


root = Path(__file__).resolve().parent.parent
source_paths = [root / 'corpora' / f'length-families-{part}.json' for part in ('a', 'b')]
pairs = [pair for path in source_paths for pair in json.loads(path.read_text(encoding='utf-8'))]
assert len(pairs) == 96
assert {(p['FamilyId'], p['TargetLength']) for p in pairs} == {
    (family, length) for family in range(1, 25) for length in (100, 200, 400, 800)}
cases = []
positions = []
for p in sorted(pairs, key=lambda p: (p['TargetLength'], p['FamilyId'])):
    family, target = p['FamilyId'], p['TargetLength']
    normal, typo = p['NormalText'], p['TypoText']
    assert target * .95 <= len(normal) <= target * 1.05, (family, target, len(normal))
    assert '\n' not in normal and '\r' not in normal
    assert len(p['Edits']) == (1 if family <= 18 else 2 if family <= 21 else 3)
    spans = []
    for e in p['Edits']:
        before, after = e['Before'], e['After']
        assert before and before != after and typo.count(before) == 1, (family, target, before)
        start = typo.index(before)
        spans.append((start, start + len(before), after))
    spans.sort()
    assert all(a[1] <= b[0] for a, b in zip(spans, spans[1:])), (family, target)
    restored = typo
    for start, end, after in reversed(spans):
        restored = restored[:start] + after + restored[end:]
    assert restored == normal, (family, target, 'edit mismatch')
    fractions = [start / len(typo) for start, _, _ in spans]
    if family <= 18:
        low, high = {'beginning': (0, .30), 'middle': (.35, .65), 'end': (.70, 1)}[p['Position']]
        assert low <= fractions[0] <= high, (family, target, fractions)
    positions.append({'FamilyId': family, 'TargetLength': target, 'NormalLength': len(normal),
                      'TypoLength': len(typo), 'ErrorPositions': fractions})
    # Declared before API evaluation: both particles give natural minimal repairs.
    alternatives = {10: ('迷惑にならない', '迷惑とならない'),
                    23: ('糸を針に通します', '糸を針へ通します')}
    expected = [normal]
    if family in alternatives:
        before, after = alternatives[family]
        assert normal.count(before) == 1
        expected.append(normal.replace(before, after))
    error_type = p['ErrorType'].replace('かな/カナ', 'かな・カナ').replace('文字/語', '文字・語')
    for is_typo, text in [(False, normal), (True, typo)]:
        cases.append({'Id': target * 100 + family * 2 + int(is_typo), 'Split': 'length-holdout',
                      'IsTypo': is_typo, 'Category': p['Topic'], 'Text': text, 'ExpectedTexts': expected if is_typo else [normal],
                      'TargetLength': target, 'FamilyId': family,
                      'ErrorType': error_type if is_typo else 'none',
                      'ErrorPosition': p['Position'] if is_typo else 'none',
                      'ErrorCount': len(p['Edits']) if is_typo else 0})
assert len({c['Text'] for c in cases}) == len(cases)
assert len({c['Id'] for c in cases}) == len(cases)
destination = root / 'corpora' / 'length-holdout-20261008.json'
destination.write_text(json.dumps(cases, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
validation = {'Cases': len(cases), 'Families': 24,
              'SourceHashes': {p.name: hashlib.sha256(p.read_bytes()).hexdigest() for p in source_paths},
              'CorpusFileSha256': hashlib.sha256(destination.read_bytes()).hexdigest(),
              'PerLength': [{'TargetLength': target, 'Normal': sum(not c['IsTypo'] for c in cases if c['TargetLength'] == target),
                             'Typo': sum(c['IsTypo'] for c in cases if c['TargetLength'] == target),
                             'MinChars': min(len(c['Text']) for c in cases if c['TargetLength'] == target),
                             'MaxChars': max(len(c['Text']) for c in cases if c['TargetLength'] == target)}
                            for target in (100, 200, 400, 800)],
              'TypoTypes': dict(Counter(c['ErrorType'] for c in cases if c['IsTypo'])), 'Pairs': positions}
(root / 'corpora' / 'length-validation.json').write_text(json.dumps(validation, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
print(json.dumps({k: v for k, v in validation.items() if k != 'Pairs'}, ensure_ascii=False, indent=2))
