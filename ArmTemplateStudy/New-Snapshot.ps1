param (
    [string] $Name
)

. $PSScriptRoot\Common\Initialize-Script.ps1

# $Nameが指定されていない場合、現在の時刻を取得し、フォーマットを設定
if (-not $Name) {
    $Name = Get-Date -Format "yyyy.MM.dd_HH.mm.ss"
}

$VirtualMachineNames | ForEach-Object -Parallel {
    # 必要な関数を再度インポート
    . $using:PSScriptRoot\Common\Common.ps1  

    # スナップショットのプレフィックスを取得
    $SnapshotPrefix = Get-SnapshotPrefix -VirtualMachineName $_

    # スナップショット名を作成（ディスク名 + タイムスタンプ）
    $snapshotName = "${SnapshotPrefix}-${using:Name}"

    # ディスクの存在を確認
    $diskName = Get-DiskName -VirtualMachineName $_
    $diskId = az disk show --name $diskName --resource-group $using:MyResourceGroup --query id -o tsv 2>$null
    if (-not $diskId) {
        Write-Error "ディスク '$diskName' が見つかりません。スクリプトを終了します。"
        exit 1
    }

    # スナップショットを作成
    Write-Output "スナップショット '$snapshotName' を作成中..."
    az snapshot create `
        --resource-group $using:ProductResourceGroup `
        --name $snapshotName `
        --source $diskId > $null

    if ($LASTEXITCODE -eq 0) {
        Write-Host -ForegroundColor Cyan "スナップショット '$snapshotName' が作成されました。"
    } else {
        Write-Host -ForegroundColor Red "スナップショット '$snapshotName' の作成に失敗しました。"
        exit 1
    }
} -ThrottleLimit 4
