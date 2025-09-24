# Repository Guidelines

- ユーザーとの対話・コードコメントなどは日本語を利用すること
- ユーザーとの対話は簡潔かつ丁寧に「です・ます」調で行うこと
- コードコメントは常体を利用すること

## Project Structure & Module Organization
- Root: scenario folders `case1/`, `case2/`, `case3/` to evaluate PowerShell zip behavior.
- Contents: `case1` has files at root; `case2` emphasizes nested folders (e.g., `child/child/`); `case3` combines both patterns.
- Docs: `README.md` explains expected archive layouts. Do not commit generated archives.

## Build, Test, and Development Commands
- Create work dirs: `New-Item -ItemType Directory -Force out,tmp | Out-Null`
- Compress case1: `Compress-Archive -Path 'case1/*' -DestinationPath 'out/case1-compress.zip' -CompressionLevel Optimal`
- Compress case2 (preserve structure): `Compress-Archive -Path 'case2' -DestinationPath 'out/case2-compress.zip' -CompressionLevel Optimal`
- Compress case3: `Compress-Archive -Path 'case3' -DestinationPath 'out/case3-compress.zip' -CompressionLevel Optimal`
- Inspect archive: `Expand-Archive 'out/case2-compress.zip' -DestinationPath 'tmp/case2' -Force; Get-ChildItem -Recurse tmp/case2`

## Coding Style & Naming Conventions
- Language: PowerShell. Place scripts in `scripts/` (if added) with `.ps1` extension.
- Functions: Verb-Noun and PascalCase (e.g., `New-CaseArchive`).
- Indentation: 2 spaces; line length ~120; single quotes for literals, double for interpolation.
- Hygiene: `Set-StrictMode -Version Latest`; avoid global state; prefer parameterized functions.
- Linting (optional): `Invoke-ScriptAnalyzer -Path scripts -Recurse` before PRs.

## Testing Guidelines
- Manual checks: use `Expand-Archive` and `Get-ChildItem -Recurse` to verify folder depth and file placement.
- Pester (optional): place tests in `tests/` as `*.Tests.ps1`; focus on path handling, archive structure, and idempotency.
- Naming: mirror source (e.g., `tests/New-CaseArchive.Tests.ps1`).

## Commit & Pull Request Guidelines
- Commits: imperative mood, clear scope (e.g., "scripts: add case2 compressor"). Explain why + what changed.
- PRs: include repro commands, expected vs. actual tree snippets, and any environment notes (`$PSVersionTable`).
- Artifacts: do not commit `out/` or `tmp/`. Add to `.gitignore` if needed.

## Security & Configuration Tips
- Only expand archives you trust; prefer controlled `tmp/` and `out/` paths.
- Use `-Force` carefully to avoid overwriting unintended files.
