$ErrorActionPreference = "Stop"

# パラメーター
$subscriptionId = "fc7753ed-2e69-4202-bb66-86ff5798b8d5"
$resourceGroupName = "rg-arm-template-study-dev-japaneast-001"
$diskName = "osdisk-vm-001-dev-japaneast"
$vhdFileName = "$diskName.vhd"
$storageAccountName = "armtemplatestudy"
$containerName = "disk-backup"

# 時間計測の共通関数
# 時間計測の共通関数
function Measure-ExecutionTime {
  param (
      [string]$operationName,
      [scriptblock]$operation
  )

  # 処理開始のメッセージを表示
  Write-Host -NoNewline "$operationName 中..."

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

# 現在のサブスクリプションIDを取得
$currentSubscriptionId = (Get-AzContext).Subscription.Id

# サブスクリプションの設定（必要な場合のみ）
if ($currentSubscriptionId -ne $subscriptionId) {
    Write-Host "現在のサブスクリプションID '$currentSubscriptionId' と指定されたサブスクリプションID '$subscriptionId' が異なります。サブスクリプションを変更します..."
    Set-AzContext -SubscriptionId $subscriptionId
}

# ストレージコンテキストの取得
$storageContext = (Get-AzStorageAccount -ResourceGroupName $resourceGroupName -Name $storageAccountName).Context

# マネージドディスクのVHDファイルへのエクスポート
$sasUri = Measure-ExecutionTime -operationName "マネージドディスク '$diskName' のエクスポート" -operation {
    Grant-AzDiskAccess -ResourceGroupName $resourceGroupName -DiskName $diskName -Access Read -DurationInSecond 86400
}

# VHDファイルをBlob Storageにコピー
Measure-ExecutionTime -operationName "VHDファイル '$vhdFileName' のコピー" -operation {
    Start-AzStorageBlobCopy -AbsoluteUri $sasUri.AccessSAS -DestContainer $containerName -DestBlob $vhdFileName -DestContext $storageContext -Force > $null
}

# SAS URI の開放を計測
Measure-ExecutionTime -operationName "SAS URI の開放" -operation {
    Revoke-AzDiskAccess -ResourceGroupName $resourceGroupName -DiskName $diskName > $null
}

Write-Host "マネージドディスク '$diskName' がVHDファイル '$vhdFileName' としてストレージアカウント '$storageAccountName' のコンテナ '$containerName' にコピーされました。"
