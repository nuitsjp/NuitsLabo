function Select-Snapshot {
    # スナップショットの一覧を取得
    Write-Host "利用可能なスナップショットを取得中..."
    $snapshots = az snapshot list --resource-group $ResourceGroup --query "[].{Name:name, TimeCreated:timeCreated, Id:id}" -o json | ConvertFrom-Json

    if ($snapshots.Count -eq 0) {
        Write-Error "利用可能なスナップショットが見つかりません。スクリプトを終了します。"
        exit 1
    }

    # スナップショットの一覧を表示
    Write-Host "利用可能なスナップショット:"
    for ($i = 0; $i -lt $snapshots.Count; $i++) {
        Write-Host "$($i + 1). $($snapshots[$i].Name) (作成日時: $($snapshots[$i].TimeCreated))"
    }

    # ユーザーにスナップショットを選択させる
    do {
        $selection = Read-Host "使用するスナップショットの番号を入力してください (1-$($snapshots.Count))"
    } while ($selection -lt 1 -or $selection -gt $snapshots.Count)

    return $snapshots[$selection - 1].Id
}

# 共通スクリプトをロード
. $PSScriptRoot\Common\Initialize-Script.ps1

# スナップショットを選択する
$SnapshotId = Select-Snapshot

# ディスクのリソースIDを取得し、スナップショットのリソースIDと比較
# ディスクのリソースIDがスナップショットのリソースIDと一致しない場合、VMを削除してディスクを作成する
$diskSourceResourceId = az disk show --name $DiskName --resource-group $ResourceGroup --query "creationData.sourceResourceId" -o tsv
if ($diskSourceResourceId -ne $snapshotId) {
    Write-Host "ディスクのソースリソースIDがスナップショットIDと一致しません。VMを削除してディスクを作成します。"
    Write-Host "VM $VirtualMachineName を削除中..."
    az vm delete --name $VirtualMachineName --resource-group $ResourceGroup --yes
}

# Bicepテンプレートをデプロイ
Write-Host "VM $VirtualMachineName を作成中..."
az deployment group create `
    --resource-group $ResourceGroup `
    --template-file "$PSScriptRoot\template\vm.bicep" `
    --parameters "$PSScriptRoot\template\vm.json" `
    --parameters snapshotId=$SnapshotId `
    --parameters virtualMachineName=$VirtualMachineName `
    --parameters diskName=$DiskName > $null

# 作成に成功したのに、失敗したとエラーがでることがあるため、VMの存在を確認
$vm = az vm show --name $VirtualMachineName --resource-group $ResourceGroup -o json | ConvertFrom-Json

if ($vm) {
    Write-Host "VM '$VirtualMachineName' を作成しました。"
} else {
    Write-Host -ForegroundColor Red "VM '$VirtualMachineName' の作成に失敗しました。"
    exit 1
}
