param (
    [string] $BackupName
)

Write-Host "共通スクリプトをロード中..."
. $PSScriptRoot\Common\Common.ps1

Write-Host "変数 virtualMachineName を設定中..."
$virtualMachineName = "vm-001"

Write-Host "既存のVM '$virtualMachineName' を確認中..."
$existingVm = Get-AzVM -ResourceGroupName $MyResourceGroup -Name $virtualMachineName -ErrorAction SilentlyContinue
if ($existingVm) {
    Write-Host "既存のVM '$virtualMachineName' が見つかりました。削除を開始します..."
    Measure-ExecutionTime -operationName "既存のVM '$virtualMachineName' を削除" -operation {
        Write-Host "VM '$virtualMachineName' を削除中..."
        Remove-AzVM -ResourceGroupName $MyResourceGroup -Name $virtualMachineName -Force
    }
}

Write-Host "ストレージアカウントキーを取得中..."
$storageAccountKey = (Get-AzStorageAccountKey -ResourceGroupName $ProductResourceGroup -Name $StorageAccountName).Value[0]

Write-Host "ストレージコンテキストを作成中..."
$context = New-AzStorageContext -StorageAccountName $StorageAccountName -StorageAccountKey $storageAccountKey

Write-Host "Blobの名前を設定中..."
$blobName = "Issue/i0001/osdisk-vm-001-dev-japaneast.vhd"

Write-Host "Blobの絶対URIを取得中..."
$blobUri = (Get-AzStorageBlob -Container $containerName -Blob $blobName -Context $context).ICloudBlob.Uri.AbsoluteUri

# Managed Diskのストレージタイプを指定します。Premium_LRSまたはStandard_LRS。
$sku = 'Standard_LRS'

# ディスクのサイズをGB単位で指定します。これはVHDファイルのサイズより大きくする必要があります。
$diskSize = '128'

Write-Host "ディスク名を取得中..."
$diskName = Get-DiskName -VirtualMachineName $virtualMachineName

# 既存のディスクをチェック
$existingDisk = Get-AzDisk -ResourceGroupName $MyResourceGroup -DiskName $diskName -ErrorAction SilentlyContinue

if ($existingDisk) {
    Write-Host "既存のディスク '$diskName' が見つかりました。既存のディスクを使用します。"
    $managedDisk = $existingDisk
} else {
    # BLOB Storageのコンテキストを取得
    $storageAccount = Get-AzStorageAccount -ResourceGroupName $ProductResourceGroup -Name $StorageAccountName
    $storageAccountId = $storageAccount.Id

    # ディスク設定を構成します
    Write-Host "Creating disk configuration for Managed Disk: $diskName"
    $diskConfig = New-AzDiskConfig  -OsType Windows -HyperVGeneration V2 -SkuName $sku -Location $location -DiskSizeGB $diskSize -SourceUri $blobUri -CreateOption Import -StorageAccountId $storageAccountId
    $diskconfig = Set-AzDiskSecurityProfile -Disk $diskconfig -SecurityType "TrustedLaunch"

    # Managed Diskを作成します
    Write-Host "Creating Managed Disk: $diskName in Resource Group: $MyResourceGroup"
    $managedDisk = New-AzDisk -DiskName $diskName -Disk $diskConfig -ResourceGroupName $MyResourceGroup
}

Write-Host "NIC名を取得中..."   
$nicName = Get-NicName -VirtualMachineName $virtualMachineName
$diskId = $managedDisk.Id

Write-Host "VM $virtualMachineName をBlob $vhdFilePath (ID: $diskId ) から復元中..."
Write-Host "Bicepテンプレートをデプロイして新しいVMを作成中..."
New-AzResourceGroupDeployment `
    -ResourceGroupName $MyResourceGroup `
    -TemplateFile "$PSScriptRoot\template\vm.bicep" `
    -TemplateParameterFile "$PSScriptRoot\template\vm.json" `
    -diskId $diskId `
    -virtualMachineName $virtualMachineName `
    -networkInterfaceName $nicName  

# Write-Host -ForegroundColor Cyan "VM '$virtualMachineName' の作成に成功しました。"