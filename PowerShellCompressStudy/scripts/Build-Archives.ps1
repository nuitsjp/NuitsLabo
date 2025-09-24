Set-StrictMode -Version Latest   # 厳格モードを有効化し、未定義参照や暗黙の型変換を検出する
$ErrorActionPreference = 'Stop'  # 例外を即座にスローして早期失敗させる

$outPath = Join-Path $PSScriptRoot "out"
if (Test-Path $outPath) {
    Remove-Item -Recurse -Force -LiteralPath $outPath
}
New-Item -ItemType Directory -Force -Path $outPath | Out-Null

$cases = @('case1', 'case2', 'case3') # ビルド対象ケース

foreach ($case in $cases) {
    $srcDir = Join-Path $PSScriptRoot ".." $case

    # トップレベルフォルダを ZIP のルートに含めないため、ワイルドカードを利用する。
    # 例: case1/* ⇒ アーカイブ直下にファイルが並ぶ。
    # もし case2 のようにトップレベルを含めたいなら、$pathPattern を $srcDir に切り替える。
    $pathPattern = Join-Path $srcDir '*'
    $zipPath = Join-Path $outPath ("{0}-compress.zip" -f $case) # 出力 ZIP のパスを決定する

    Write-Output "source: $pathPattern => zip: $zipPath"

    Compress-Archive -Path $pathPattern -DestinationPath $zipPath -CompressionLevel Optimal -Force # ZIP を作成する
}
