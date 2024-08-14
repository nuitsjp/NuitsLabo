. $PSScriptRoot\Common\Common.ps1

# パラメーター
$diskName = "osdisk-vm-001-dev-japaneast"
$snapshotName = "$diskName-temporary-snapshot"
$vhdFileName = "$diskName.vhd"

# 完全なスナップショットの作成
Measure-ExecutionTime -operationName "完全スナップショット '$snapshotName' の作成" -operation {
  $snapshotConfig = New-AzSnapshotConfig -SourceUri (Get-AzDisk -ResourceGroup $MyResourceGroup -DiskName $diskName).Id -Location "japaneast" -CreateOption Copy
  New-AzSnapshot -ResourceGroup $MyResourceGroup -SnapshotName $snapshotName -Snapshot $snapshotConfig > $null
}

Write-Host "スナップショットの作成が完了しました。完了を待たずVMを利用して問題ありません。" -ForegroundColor Cyan

try {
  # スナップショットのSAS URIの取得
  $sasUri = Measure-ExecutionTime -operationName "スナップショット '$snapshotName' のエクスポート" -operation {
    Grant-AzSnapshotAccess -ResourceGroup $MyResourceGroup -SnapshotName $snapshotName -Access Read -DurationInSecond 300
  }

  try {
    # スナップショットをBlob Storageにコピー
    $storageContext = (Get-AzStorageAccount -ResourceGroup $MyResourceGroup -Name $StorageAccountName).Context
    Measure-ExecutionTime -operationName "VHDファイル '$vhdFileName' のコピー" -operation {
      Start-AzStorageBlobCopy -AbsoluteUri $sasUri.AccessSAS -DestContainer $ContainerName -DestBlob $vhdFileName -DestContext $storageContext -Force > $null
    }
  }
  finally {
    # SAS URI の開放
    Measure-ExecutionTime -operationName "SAS URI の開放" -operation {
      Revoke-AzSnapshotAccess -ResourceGroup $MyResourceGroup -SnapshotName $snapshotName > $null
    }
  }
}
finally {
  # スナップショットの削除
  Measure-ExecutionTime -operationName "スナップショット '$snapshotName' の削除" -operation {
    Remove-AzSnapshot -ResourceGroup $MyResourceGroup -SnapshotName $snapshotName -Force > $null
  }
}

Write-Host "完全なスナップショットの作成、エクスポート、削除が完了しました。VHDファイル '$vhdFileName' がストレージアカウント '$StorageAccountName' のコンテナ '$ContainerName' にコピーされました。"

