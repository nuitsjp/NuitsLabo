<#
.SYNOPSIS
    BLOB StorageからManaged Diskを復元するPowerShellスクリプト。

.DESCRIPTION
    このスクリプトは、指定されたBLOB StorageからVHDファイルを取得し、それを使用して新しいManaged Diskを作成します。
    スクリプトは以下のステップで動作します：
    1. 復元するバックアップ名を指定します。
    2. BLOB StorageからVHDファイルのURLを取得します。
    3. VHDファイルからManaged Diskを作成します。
    4. 作成したManaged Diskを指定された仮想マシンにアタッチします（オプション）。

.PARAMETER BackupName
    復元するバックアップの名前を指定します。

.PARAMETER VirtualMachineName
    （オプション）復元したディスクをアタッチする仮想マシンの名前を指定します。

.EXAMPLE
    .\Restore-VMDisk.ps1 -BackupName "Release/v1.0.0"

.EXAMPLE
    .\Restore-VMDisk.ps1 -BackupName "Issue/i0123" -VirtualMachineName "VM001"
#>

# 共通の関数や設定を含むスクリプトをインポート
. $PSScriptRoot\Common\Common.ps1

# 実行時間を計測する関数
function Measure-ExecutionTime {
    param (
        [string] $operationName,
        [scriptblock] $operation
    )
    $startTime = Get-Date
    & $operation
    $endTime = Get-Date
    $duration = $endTime - $startTime
    Write-Host "$operationName の実行時間: $($duration.TotalSeconds) 秒" -ForegroundColor Yellow
}

# バックアップ復元処理の実行時間を計測しながら処理を実行
Measure-ExecutionTime -operationName "'$BackupName' の復元" -operation {
    # BLOB Storageのコンテキストを取得
    $storageAccount = Get-AzStorageAccount -ResourceGroupName $ProductResourceGroup -Name $StorageAccountName
    $storageContext = $storageAccount.Context

    # ストレージアカウントのIDを取得
    $storageAccountId = $storageAccount.Id

    # コンテナ内のBLOBを一覧表示
    $blobs = Get-AzStorageBlob -Container $ContainerName -Context $storageContext
    $blobs

    foreach ($blob in $blobs) {
        $diskName = "vm-001"

        # 1/3: BLOBのURLを取得
        $blobUrl = Measure-ExecutionTime -operationName "'$diskName' [1/3] BLOBのURL取得" -operation {
            $blob.ICloudBlob.Uri.AbsoluteUri
        }
        $blobUrl

        # 2/3: Managed Diskの作成
        $disk = Measure-ExecutionTime -operationName "'$diskName' [2/3] Managed Diskの作成" -operation {
            $diskConfig = New-AzDiskConfig -AccountType Premium_LRS -Location $Location -CreateOption Import -SourceUri $blobUrl -StorageAccountId $storageAccountId
            $diskName = "vm-001"
            New-AzDisk -ResourceGroupName $MyResourceGroup -DiskName $diskName -Disk $diskConfig
        }
        $disk

        # 3/3: 仮想マシンへのディスクのアタッチ（オプション）
        Measure-ExecutionTime -operationName "'$diskName' [3/3] 仮想マシンへのディスクのアタッチ" -operation {
            $deploymentName = "Deploy-" + (Get-Date).ToString("yyyyMMdd-HHmmss")

            $templateFile = "$PSScriptRoot\template\vm.bicep"
            $parameterFile = "$PSScriptRoot\template\vm.json"
            $virtualMachineName = "vm-001"
            $nicName = Get-NicName -VirtualMachineName $virtualMachineName

            $additionalParameters = @{
                diskId = $disk.Id
                virtualMachineName = $virtualMachineName
                networkInterfaceName = $nicName
            }
            $diskId = $disk.Id
            
            az deployment group create `
                --resource-group $MyResourceGroup `
                --template-file "$PSScriptRoot\template\vm.bicep" `
                --parameters "$PSScriptRoot\template\vm.json" `
                --parameters diskId=$diskId `
                --parameters virtualMachineName=$virtualMachineName `
                --parameters networkInterfaceName=$nicName 2>&1 | Out-Null # 作成に成功したのに、失敗したとエラーがでることがあるためコンソール出力を抑止

            # New-AzResourceGroupDeployment `
            #     -Name $deploymentName `
            #     -ResourceGroupName $MyResourceGroup `
            #     -TemplateFile $templateFile `
            #     -TemplateParameterFile $parameterFile `
            #     -TemplateParameterObject $additionalParameters `
        }

        Write-Host "'$diskName' を復元し、Managed Diskとして作成しました。" -ForegroundColor Cyan
    }
}