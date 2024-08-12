function Select-SnapshotInstance {
    param (
        [string]$Prefix
    )

    # スナップショットの一覧を取得し、プレフィックスに一致するものをフィルタリング
    Write-Host "利用可能なスナップショットを取得中..."
    $filteredSnapshots = az snapshot list --resource-group $ProductResourceGroup --query "[].{Name:name, TimeCreated:timeCreated, Id:id}" -o json |
                         ConvertFrom-Json | Where-Object { $_.Name -like "$Prefix*" }

    # フィルタリングされたスナップショットがない場合、エラーを表示して終了
    if (-not $filteredSnapshots) {
        Write-Error "スナップショットが見つかりません。スクリプトを終了します。"
        exit 1
    }

    # スナップショットの一覧を表示（サフィックスのみ）
    Write-Host "利用可能なスナップショット (プレフィックス: $Prefix):"
    $suffixes = @()
    for ($i = 0; $i -lt $filteredSnapshots.Count; $i++) {
        $suffix = $filteredSnapshots[$i].Name.Substring($Prefix.Length + 1)
        $suffixes += $suffix
        Write-Host "$($i + 1). $suffix (作成日時: $($filteredSnapshots[$i].TimeCreated))"
    }

    # ユーザーにスナップショットを選択させる
    do {
        $selection = Read-Host "使用するスナップショットの番号を入力してください (1-$($filteredSnapshots.Count))"
    } while ($selection -lt 1 -or $selection -gt $filteredSnapshots.Count)

    # 選択されたスナップショットのサフィックスを返す
    return $suffixes[$selection - 1]
}

# 共通スクリプトをロード
. $PSScriptRoot\Common\Initialize-Script.ps1

# スナップショットインスタンスを選択する
$snapshotInstance = Select-SnapshotInstance -Prefix (Get-SnapshotPrefix -VirtualMachineName $VirtualMachineNames[0])

# 並列で処理を実行する
$VirtualMachineNames | ForEach-Object -Parallel {
    # 必要な関数を再度インポート
    . $using:PSScriptRoot\Common\Common.ps1  

    $virtualMachineName = $_

    # スナップショットを取得
    $snapshot = Get-Snapshot -Prefix (Get-SnapshotPrefix -VirtualMachineName $virtualMachineName) -Instance $using:snapshotInstance
    $snapshotId = $snapshot.Id

    # ディスクのリソースIDを取得し、スナップショットのリソースIDと比較
    # ディスクのリソースIDがスナップショットのリソースIDと一致しない場合、VMを削除してディスクを作成する
    $diskName = Get-DiskName -VirtualMachineName $virtualMachineName
    $disk = az disk list --resource-group $using:MyResourceGroup --query "[?name=='$diskName'] | [0].{Id:id, SourceResourceId:creationData.sourceResourceId}" -o json | ConvertFrom-Json
    if ($disk) {
        # ディスクが存在し、その sourceResourceId がスナップショットと一致しない場合にVMを削除
        if ($disk.SourceResourceId -ne $snapshotId) {
            Write-Host "ディスク '$diskName' のソースリソースIDがスナップショットと一致しません。VM '$virtualMachineName' を削除中..."
            az vm delete --name $virtualMachineName --resource-group $using:MyResourceGroup --yes
        }
    }

    # Bicepテンプレートをデプロイ
    Write-Host "VM $virtualMachineName をスナップショット $($snapshot.Name) から復元中..."
    $nicName = Get-NicName -VirtualMachineName $virtualMachineName
    az deployment group create `
        --resource-group $using:MyResourceGroup `
        --template-file "$using:PSScriptRoot\template\vm.bicep" `
        --parameters "$using:PSScriptRoot\template\vm.json" `
        --parameters snapshotId=$snapshotId `
        --parameters virtualMachineName=$virtualMachineName `
        --parameters diskName=$diskName `
        --parameters networkInterfaceName=$nicName 2>&1 | Out-Null

    # 作成に成功したのに、失敗したとエラーがでることがあるため、VMの存在を確認
    $vm = az vm show --name $virtualMachineName --resource-group $using:MyResourceGroup -o json | ConvertFrom-Json
    if ($vm) {
        Write-Host -ForegroundColor Cyan "VM '$virtualMachineName' の作成に成功しました。"
    } else {
        Write-Host -ForegroundColor Red "VM '$virtualMachineName' の作成に失敗しました。"
        exit 1
    }
}
