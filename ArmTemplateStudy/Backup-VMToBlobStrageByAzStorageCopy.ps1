$ErrorActionPreference = "Stop"

# パラメーター
$subscriptionId = "fc7753ed-2e69-4202-bb66-86ff5798b8d5"
$resourceGroupName = "rg-arm-template-study-dev-japaneast-001"
$diskName = "osdisk-vm-001-dev-japaneast"
$vhdFileName = "$diskName.vhd"
$storageAccountName = "armtemplatestudy"
$containerName = "disk-backup"

# サブスクリプションを設定
Write-Host "サブスクリプションを設定中..."
az account set --subscription $subscriptionId

# ストレージアカウントキーを取得
Write-Output "ストレージアカウントキーを取得中..."
$storageAccountKey = az storage account keys list `
  --resource-group $resourceGroupName `
  --account-name $storageAccountName `
  --query "[0].value" `
  --output tsv

# VHDエクスポートとコピーの時間計測開始
$startTime = Get-Date

# Managed DiskをVHDファイルにエクスポート
Write-Host "Managed Disk $diskName をVHDファイルにエクスポート中..."
$sas = az disk grant-access `
  --resource-group $resourceGroupName `
  --name $diskName `
  --duration-in-seconds 3600 `
  --access-level Read `
  --query "accessSas" `
  --output tsv
$sas = '"' + $sas + '"'

$endTime = Get-Date
$elapsedTime = $endTime - $startTime

Write-Output "エクスポートにかかった時間: $($elapsedTime.TotalSeconds) 秒"

$startTime = Get-Date

# Blob Storageコピー
Write-Host "VHDファイルをコピー中..."
az storage blob copy start `
  --account-key $storageAccountKey `
  --account-name $storageAccountName `
  --destination-container $containerName `
  --destination-blob $vhdFileName `
  --source-uri $sas

# VHDエクスポートとコピーの時間計測終了
$endTime = Get-Date
$elapsedTime = $endTime - $startTime

Write-Output "コピーにかかった時間: $($elapsedTime.TotalSeconds) 秒"