$ErrorActionPreference = "Stop"

# パラメーター
$subscriptionId = "fc7753ed-2e69-4202-bb66-86ff5798b8d5"
$resourceGroupName = "rg-arm-template-study-dev-japaneast-001"
$storageAccountName = "armtemplatestudy"
$containerName = "disk-backup"

# 現在のサブスクリプションIDを取得
$currentSubscriptionId = (Get-AzContext).Subscription.Id

# サブスクリプションの設定（必要な場合のみ）
if ($currentSubscriptionId -ne $subscriptionId) {
    Write-Host "現在のサブスクリプションID '$currentSubscriptionId' と指定されたサブスクリプションID '$subscriptionId' が異なります。サブスクリプションを変更します..."
    Set-AzContext -SubscriptionId $subscriptionId
}

# ストレージアカウントとコンテナの存在確認と作成
Write-Host "ストレージアカウント '$storageAccountName' の存在を確認しています..."
$storageAccount = Get-AzStorageAccount -ResourceGroupName $resourceGroupName -Name $storageAccountName -ErrorAction SilentlyContinue
if (-not $storageAccount) {
    Write-Host "ストレージアカウント '$storageAccountName' が見つかりません。作成します。"
    $storageAccount = New-AzStorageAccount -ResourceGroupName $resourceGroupName -Name $storageAccountName -SkuName "Standard_LRS" -Location "japaneast"
}

$storageContext = $storageAccount.Context

Write-Host "コンテナ '$containerName' の存在を確認しています..."
$container = Get-AzStorageContainer -Context $storageContext -Name $containerName -ErrorAction SilentlyContinue
if (-not $container) {
    Write-Host "コンテナ '$containerName' が見つかりません。作成します。"
    New-AzStorageContainer -Context $storageContext -Name $containerName
}