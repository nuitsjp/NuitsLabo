. $PSScriptRoot\Common\Initialize-Script.ps1

# 現在の日時を取得し、フォーマットを設定
$timestamp = Get-Date -Format "yyyy.MM.dd_HH.mm.ss"

foreach ($virtualMachineName in $VirtualMachineNames) {
    # スナップショットのプレフィックスを取得
    $SnapshotPrefix = Get-SnapshotPrefix -VirtualMachineName $virtualMachineName

    # スナップショット名を作成（ディスク名 + タイムスタンプ）
    $snapshotName = "${SnapshotPrefix}-${timestamp}"

    # ディスクの存在を確認
    $diskName = Get-DiskName -VirtualMachineName $virtualMachineName
    $diskId = az disk show --name $diskName --resource-group $MyResourceGroup --query id -o tsv 2>$null
    if (-not $diskId) {
        Write-Error "ディスク '$diskName' が見つかりません。スクリプトを終了します。"
        exit 1
    }

    # スナップショットを作成
    Write-Output "スナップショット '$snapshotName' を作成中..."
    az snapshot create `
        --resource-group $ProductResourceGroup `
        --name $snapshotName `
        --location $Location `
        --source $diskId > $null

    if ($LASTEXITCODE -eq 0) {
        Write-Host -ForegroundColor Cyan "スナップショット '$snapshotName' が作成されました。"
    } else {
        Write-Host -ForegroundColor Red "スナップショット '$snapshotName' の作成に失敗しました。"
        exit 1
    }
}