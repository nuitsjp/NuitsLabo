# 共通スクリプトをロード
. $PSScriptRoot\Common\Initialize-Script.ps1

# スナップショットを選択する
$selectedSnapshot = Select-Snapshot

# 選択されたスナップショットを使用してディスクを復元
$deployment = Restore-Disk -SnapshotName $selectedSnapshot
Write-Host $deployment