$ErrorActionPreference = "Stop"

# パラメーター
$subscriptionId = "fc7753ed-2e69-4202-bb66-86ff5798b8d5"
$resourceGroupName = "rg-arm-template-study-dev-japaneast-001"
$location = "japaneast"
$diskName = "osdisk-vm-001-dev-japaneast"
$vhdFileName = "$diskName.vhd"
$storageAccountName = "armtemplatestudy"
$containerName = "disk-backup"

# サブスクリプションを設定
az account set --subscription $subscriptionId

# ストレージアカウントキーを取得
$storageAccountKey = az storage account keys list `
  --resource-group $resourceGroupName `
  --account-name $storageAccountName `
  --query "[0].value" `
  --output tsv

# VHDエクスポートとコピーの時間計測開始
$startTime = Get-Date

# Managed DiskをVHDファイルにエクスポート
Write-Host "Managed Disk $diskName をVHDファイルにエクスポート中..."
$sourceUrl = az disk grant-access `
  --resource-group $resourceGroupName `
  --name $diskName `
  --duration-in-seconds 3600 `
  --access-level Read `
  --query "accessSas" `
  --output tsv

$endTime = Get-Date
$elapsedTime = $endTime - $startTime

Write-Output "エクスポートにかかった時間: $($elapsedTime.TotalSeconds) 秒"

$startTime = Get-Date

# 書き込み用のSAS URLを取得
Write-Host "書き込み用のSAS URLを取得中..."
$writeSasToken = az storage blob generate-sas `
  --account-key $storageAccountKey `
  --account-name $storageAccountName `
  --container-name $containerName `
  --name $vhdFileName `
  --permissions "w" `
  --expiry (Get-Date).AddHours(1).ToString("yyyy-MM-ddTHH:mmZ") `
  --https-only `
  --output tsv

$destinationUrl = "https://$storageAccountName.blob.core.windows.net/$($containerName)/$($vhdFileName)?$($writeSasToken)"

# AzCopyでVHDファイルをコピー
Write-Host "AzCopyを使用してVHDファイルをコピー中..."
azcopy copy $sourceUrl $destinationUrl

# VHDエクスポートとコピーの時間計測終了
$endTime = Get-Date
$elapsedTime = $endTime - $startTime

Write-Output "コピーにかかった時間: $($elapsedTime.TotalSeconds) 秒"