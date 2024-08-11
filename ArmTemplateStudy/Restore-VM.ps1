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
$restoredDisk = Restore-Disk -SnapshotName $selectedSnapshot

# 作成されたディスクの詳細を表示
Write-Host "作成されたディスクの詳細:"
az disk show --name $DiskName --resource-group $ResourceGroup --query "{Name:name, Location:location, ProvisioningState:provisioningState, DiskState:diskState}" -o table