. $PSScriptRoot\Common\Initialize-Script.ps1

# 定数の設定
Set-Variable -Name SnapshotPrefix -Value "snp-vm-001-dev-japaneast" -Option ReadOnly -Scope Script

# 現在の日時を取得し、フォーマットを設定
$timestamp = Get-Date -Format "yyyyMMddHHmmss"

# スナップショット名を作成（ディスク名 + タイムスタンプ）
$snapshotName = "${SnapshotPrefix}-${timestamp}"

# ディスクの存在を確認
$diskId = az disk show --name $DiskName --resource-group $ResourceGroup --query id -o tsv 2>$null
if (-not $diskId) {
    Write-Error "ディスク '$DiskName' が見つかりません。スクリプトを終了します。"
    exit 1
}

# スナップショットを作成
Write-Output "スナップショット '$snapshotName' を作成中..."
az snapshot create `
    --resource-group $ResourceGroup `
    --name $snapshotName `
    --location $Location `
    --source $diskId > $null

if ($LASTEXITCODE -eq 0) {
    Write-Output "スナップショット '$snapshotName' がディスク '$DiskName' から正常に作成されました。"
} else {
    Write-Error "スナップショットの作成に失敗しました。"
    exit 1
}
