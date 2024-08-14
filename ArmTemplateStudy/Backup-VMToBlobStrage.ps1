param (
    [string] $BackupName
)

. $PSScriptRoot\Common\Common.ps1  

# $BackupNameが指定されていない場合、現在の時刻を取得し、フォーマットを設定
if (-not $BackupName) {
  $BackupName = Get-Date -Format "yyyy.MM.dd_HH.mm.ss"
}

Measure-ExecutionTime -operationName "'$BackupName' のバックアップ" -operation {
  $VirtualMachineNames | ForEach-Object -Parallel {
    # 必要な関数を再度インポート
    . $using:PSScriptRoot\Common\Common.ps1  
  
      # パラメーター
    $virtualMachineName = $_
    $diskName = Get-DiskName -VirtualMachineName $virtualMachineName
    $snapshotName = "$diskName-$using:BackupName"
    $vhdFileName = "$snapshotName.vhd"
    
    # 完全なスナップショットの作成
    Measure-ExecutionTime -operationName "'$virtualMachineName' [1/5] スナップショットの作成" -operation {
      $snapshotConfig = New-AzSnapshotConfig -SourceUri (Get-AzDisk -ResourceGroup $MyResourceGroup -DiskName $diskName).Id -Location "japaneast" -CreateOption Copy
      New-AzSnapshot -ResourceGroup $MyResourceGroup -SnapshotName $snapshotName -Snapshot $snapshotConfig > $null
    }
    
    try {
      # スナップショットのSAS URIの取得
      $sasUri = Measure-ExecutionTime -operationName "'$virtualMachineName' [2/5] スナップショットのエクスポート" -operation {
        Grant-AzSnapshotAccess -ResourceGroup $MyResourceGroup -SnapshotName $snapshotName -Access Read -DurationInSecond 300
      }
    
      try {
        # スナップショットをBlob Storageにコピー
        $storageContext = (Get-AzStorageAccount -ResourceGroup $ProductResourceGroup -Name $StorageAccountName).Context
        Measure-ExecutionTime -operationName "'$virtualMachineName' [3/5] スナップショットのコピー" -operation {
          Start-AzStorageBlobCopy -AbsoluteUri $sasUri.AccessSAS -DestContainer $ContainerName -DestBlob $vhdFileName -DestContext $storageContext -Force > $null
        }
      }
      finally {
        # SAS URI の開放
        Measure-ExecutionTime -operationName "'$virtualMachineName' [4/5] スナップショットのSAS URI の取消" -operation {
          Revoke-AzSnapshotAccess -ResourceGroup $MyResourceGroup -SnapshotName $snapshotName > $null
        }
      }
    }
    finally {
      # スナップショットの削除
      Measure-ExecutionTime -operationName "'$virtualMachineName' [5/5] スナップショットの削除" -operation {
        Remove-AzSnapshot -ResourceGroup $MyResourceGroup -SnapshotName $snapshotName -Force > $null
      }
    }
    
    Write-Host "'$virtualMachineName' を '$vhdFileName' にバックアップしました。" -ForegroundColor Cyan
  } -ThrottleLimit 4
}
