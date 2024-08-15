param (
    [string] $BackupName
)

# 共通スクリプトをロード
. $PSScriptRoot\Common\Common.ps1

# 並列で処理を実行する
$virtualMachineName = "vm-001"

# 既存のVMが存在する場合、それを削除
$existingVm = Get-AzVM -ResourceGroupName $MyResourceGroup -Name $virtualMachineName -ErrorAction SilentlyContinue
if ($existingVm) {
    Measure-ExecutionTime -operationName "既存のVM '$virtualMachineName' を削除" -operation {
        Remove-AzVM -ResourceGroupName $MyResourceGroup -Name $virtualMachineName -Force
    }
}

$diskName = Get-DiskName -VirtualMachineName $virtualMachineName
$nicName = Get-NicName -VirtualMachineName $virtualMachineName

# BlobストレージからSASトークンを生成
$storageAccountKey = (Get-AzStorageAccountKey -ResourceGroupName $ProductResourceGroup -Name $StorageAccountName).Value[0]
$context = New-AzStorageContext -StorageAccountName $StorageAccountName -StorageAccountKey $storageAccountKey

# VHDファイルのパス
#$blobName = "$($BackupName)/$($diskName).vhd"
$blobName = "abcd"

# SASトークン付きの完全なURIを取得
# $sasToken = New-AzStorageBlobSASToken -Container $containerName -Blob $blobName -Permission r -Context $context -ExpiryTime (Get-Date).AddHours(1)
# $blobUri = (Get-AzStorageBlob -Container $containerName -Blob $blobName -Context $context).ICloudBlob.Uri.AbsoluteUri
# $sasUri = "$($blobUri)?$sasToken"

# $sasUri
$sasUri = 'https://armtemplatestudy.blob.core.windows.net/disk-backup/abcd?sp=r&st=2024-08-15T04:06:34Z&se=2024-08-15T12:06:34Z&spr=https&sv=2022-11-02&sr=b&sig=7RiSHuIcvYlj4mnB40kFRHJodZdqq4aNw%2Bm6j73z6QI%3D'

# マネージドディスクの作成
# $diskConfig = New-AzDiskConfig -SkuName "Standard_LRS" -Location $Location -CreateOption Import -SourceUri $sasUri
# $managedDisk = New-AzDisk -ResourceGroupName $MyResourceGroup -DiskName $diskName -Disk $diskConfig

# Bicepテンプレートをデプロイして新しいVMを作成
Write-Host "VM $virtualMachineName をBlob $vhdFilePath から復元中..."
New-AzResourceGroupDeployment -ResourceGroupName $MyResourceGroup `
    -TemplateFile "$PSScriptRoot\template\vm.bicep" `
    -TemplateParameterFile "$PSScriptRoot\template\vm.json" `
    -snapshotId $sasUri `
    -virtualMachineName $virtualMachineName `
    -diskName $diskName `
    -networkInterfaceName $nicName

Write-Host -ForegroundColor Cyan "VM '$virtualMachineName' の作成に成功しました。"
