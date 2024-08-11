# 共通スクリプトをロード
. $PSScriptRoot\Common\Initialize-Script.ps1

# $deployment = az deployment group create `
#     --resource-group $ResourceGroup `
#     --template-file "$PSScriptRoot\template\network-interface.bicep" `
#     --parameters "$PSScriptRoot\template\network-interface.json" `
#     --query properties.outputs

# スナップショットを選択する
$selectedSnapshot = Select-Snapshot

# 選択されたスナップショットを使用してディスクを復元
$deployment = Restore-Disk -SnapshotName $selectedSnapshot
Write-Host $deployment
