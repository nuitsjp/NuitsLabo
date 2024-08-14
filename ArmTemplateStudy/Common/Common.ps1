function Get-SnapshotPrefix {
    param (
        [Parameter(Mandatory = $true)]
        [string] $VirtualMachineName
    )
    return "snp-$VirtualMachineName-dev-japaneast"
}

function Get-Snapshot {
    param (
        [string]$Prefix,
        [string]$Instance
    )

    # スナップショットの名前を結合
    $snapshotName = "${Prefix}-${Instance}"

    # スナップショットのIDと名称を取得
    $snapshot = az snapshot show --name $snapshotName --resource-group $ProductResourceGroup --query "{Name:name, Id:id}" -o json | ConvertFrom-Json
    if ($snapshot.Count -eq 0) {
        Write-Error "スナップショット '$snapshotName' が見つかりません。スクリプトを終了します。"
        exit 1
    }

    # スナップショットのIDを返す
    return $snapshot[0]
}

function Get-DiskName {
    param (
        [Parameter(Mandatory = $true)]
        [string] $VirtualMachineName
    )
    return "osdisk-$VirtualMachineName-dev-japaneast"
}

function Get-NicName {
    param (
        [Parameter(Mandatory = $true)]
        [string] $VirtualMachineName
    )
    return "nic-$VirtualMachineName-dev-japaneast"
}

# 時間計測の共通関数
function Measure-ExecutionTime {
    param (
        [string]$operationName,
        [scriptblock]$operation
    )

    # 処理開始のメッセージを表示
    Write-Host -NoNewline "$operationName 実施中..."

    # 実際の処理を実行
    $startTime = Get-Date
    $result = & $operation
    $endTime = Get-Date

    # 経過時間を計算
    $duration = $endTime - $startTime

    # 行頭に戻ってメッセージを上書き
    Write-Host "`r$operationName 完了: 経過時間 $($duration.TotalSeconds) 秒"

    return $result
}

$ErrorActionPreference = "Stop"

# 読み取り専用変数（定数）を定義
Set-Variable -Name SubscriptionId -Value "fc7753ed-2e69-4202-bb66-86ff5798b8d5" -Option ReadOnly -Scope Script
Set-Variable -Name ProductResourceGroup -Value "rg-arm-template-study-dev-japaneast-product" -Option ReadOnly -Scope Script
Set-Variable -Name MyResourceGroup -Value "rg-arm-template-study-dev-japaneast-001" -Option ReadOnly -Scope Script
Set-Variable -Name StorageAccountName -Value "armtemplatestudy" -Option ReadOnly -Scope Script
Set-Variable -Name ContainerName -Value "disk-backup" -Option ReadOnly -Scope Script
Set-Variable -Name VirtualMachineNames -Value @("vm-001", "vm-002") -Option ReadOnly -Scope Script

# サブスクリプションの設定（必要な場合のみ）
$currentSubscriptionId = (Get-AzContext).Subscription.Id
if ($currentSubscriptionId -ne $SubscriptionId) {
    Write-Host "現在のサブスクリプションID '$currentSubscriptionId' と指定されたサブスクリプションID '$SubscriptionId' が異なります。サブスクリプションを変更します..."
    Set-AzContext -SubscriptionId $SubscriptionId
}
