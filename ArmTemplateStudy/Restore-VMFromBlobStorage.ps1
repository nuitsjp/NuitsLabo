param (
  [string] $Version,
  [int] $Issue,
  [string] $BackupName
)

Write-Host "共通スクリプトをロード中..."
. $PSScriptRoot\Common\Common.ps1

# いずれか1つだけ指定されていることを確認する
if (($PSBoundParameters.Count -ne 1) -or (-not $PSBoundParameters.ContainsKey('Version') -and -not $PSBoundParameters.ContainsKey('Issue') -and -not $PSBoundParameters.ContainsKey('BackupName'))) {
    throw "Version, Issue, BackupNameのいずれか1つだけを指定してください。複数指定または未指定の場合はエラーとなります。"
}

# BackupNameを設定
if ($Version) {
  $BackupName = "Release/v$Version"
} elseif ($Issue) {
  # Issue番号を4桁のゼロ埋めにしてBackupNameを設定
  $BackupName = "Issue/i{0:D4}" -f $Issue
}

Measure-ExecutionTime -operationName "'$BackupName' の復元" -operation {
    $VirtualMachineNames | ForEach-Object -Parallel {
        # 並列実行のため、必要な関数を再度インポート
        . $using:PSScriptRoot\Common\Common.ps1

        # 現在の仮想マシン名を設定
        $virtualMachineName = $_

        Write-Host "既存のVM '$virtualMachineName' を確認中..."
        $existingVm = Get-AzVM -ResourceGroupName $MyResourceGroup -Name $virtualMachineName -ErrorAction SilentlyContinue
        if ($existingVm) {
            Measure-ExecutionTime -operationName "既存VM '$virtualMachineName' の削除" -operation {
                Remove-AzVM -ResourceGroupName $MyResourceGroup -Name $virtualMachineName -Force
            }
        }

        Write-Host "ストレージアカウントキーを取得中..."
        $storageAccountKey = (Get-AzStorageAccountKey -ResourceGroupName $ProductResourceGroup -Name $StorageAccountName).Value[0]

        Write-Host "ストレージコンテキストを作成中..."
        $context = New-AzStorageContext -StorageAccountName $StorageAccountName -StorageAccountKey $storageAccountKey

        Write-Host "Blobの名前を設定中..."
        # 仮想マシンに対応するディスク名を取得
        $diskName = Get-DiskName -VirtualMachineName $virtualMachineName
        $blobName = "$using:BackupName/$($diskName).vhd"

        Write-Host "Blobの絶対URIを取得中..."
        $blobUri = (Get-AzStorageBlob -Container $containerName -Blob $blobName -Context $context).ICloudBlob.Uri.AbsoluteUri

        # Managed Diskのストレージタイプを指定します。Premium_LRSまたはStandard_LRS。
        $sku = 'Standard_LRS'

        # ディスクのサイズをGB単位で指定します。これはVHDファイルのサイズより大きくする必要があります。
        $diskSize = '128'

        # 既存のディスクをチェック
        $existingDisk = Get-AzDisk -ResourceGroupName $MyResourceGroup -DiskName $diskName -ErrorAction SilentlyContinue

        if ($existingDisk) {
            Write-Host "既存のディスク '$diskName' が見つかりました。既存のディスクを使用します。"
            $managedDisk = $existingDisk
        } else {
            $managedDisk = Measure-ExecutionTime -operationName "ディスク '$diskName' を作成" -operation {
                # BLOB Storageのコンテキストを取得
                $storageAccount = Get-AzStorageAccount -ResourceGroupName $ProductResourceGroup -Name $StorageAccountName
                $storageAccountId = $storageAccount.Id

                # ディスク設定を構成します
                $diskConfig = New-AzDiskConfig  -OsType Windows -HyperVGeneration V2 -SkuName $sku -Location $location -DiskSizeGB $diskSize -SourceUri $blobUri -CreateOption Import -StorageAccountId $storageAccountId
                $diskconfig = Set-AzDiskSecurityProfile -Disk $diskconfig -SecurityType "TrustedLaunch"

                # Managed Diskを作成します
                New-AzDisk -DiskName $diskName -Disk $diskConfig -ResourceGroupName $MyResourceGroup
            }
        }

        $publicIpAddressName = "pip-$virtualMachineName-dev-japaneast"
        $nicName = Get-NicName -VirtualMachineName $virtualMachineName
        $diskId = $managedDisk.Id

        Measure-ExecutionTime -operationName "VM '$virtualMachineName' IP:'$publicIpAddressName' nic:'$nicName' disk:'$diskId' を復元" -operation {
            New-AzResourceGroupDeployment `
                -ResourceGroupName $MyResourceGroup `
                -TemplateFile "$using:PSScriptRoot\template\vm.bicep" `
                -TemplateParameterFile "$using:PSScriptRoot\template\vm.json" `
                -diskId $diskId `
                -virtualMachineName $virtualMachineName `
                -publicIpAddressName $publicIpAddressName `
                -networkInterfaceName $nicName > $null
            }
    } -ThrottleLimit 4
}

