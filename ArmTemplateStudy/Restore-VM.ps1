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

function Restore-Disk {
    param (
        [Parameter(Mandatory=$true)]
        [string]$SnapshotId
    )

}

# 共通スクリプトをロード
. $PSScriptRoot\Common\Initialize-Script.ps1

# スナップショットを選択する
$SnapshotId = Select-Snapshot

# パラメーターファイルを読み込む
$parameterFilePath = "$PSScriptRoot\template\vm.json"
$parameters = Get-Content $parameterFilePath | ConvertFrom-Json

# スナップショットのリソースIDを更新
$parameters.parameters.sourceResourceId.value = $SnapshotId

# 更新したパラメーターを一時ファイルに保存
$tempParameterFile = [System.IO.Path]::GetTempFileName()
$parameters | ConvertTo-Json -Depth 10 | Set-Content $tempParameterFile
try {
    # Bicepテンプレートをデプロイ
    Write-Host "Bicepテンプレートを使用してディスク '$DiskName' を作成中..."
    $deployment = az deployment group create `
        --resource-group $ResourceGroup `
        --template-file "$PSScriptRoot\template\vm.bicep" `
        --parameters "@$tempParameterFile" `
        --query properties.outputs

    if ($LASTEXITCODE -eq 0) {
        Write-Host "ディスク '$DiskName' をスナップショットから正常に作成しました。"
    } else {
        Write-Error "ディスク '$DiskName' の作成に失敗しました。"
        exit 1
    }

    return $deployment
}
finally {
    # 一時ファイルを削除
    Remove-Item $tempParameterFile
}
