<#
.SYNOPSIS
    仮想マシンのディスクをスナップショットとして取得し、それをBlob StorageにバックアップするPowerShellスクリプト。

.DESCRIPTION
    このスクリプトは、指定された仮想マシンのディスクをスナップショットとして作成し、そのスナップショットをAzure Blob Storageにバックアップする一連の処理を行います。スクリプトは以下のステップで動作します。

    1. バックアップ名が指定されていない場合、現在の日付と時刻を基にバックアップ名を生成します。
    2. 指定された仮想マシンに対して並列処理を行い、それぞれのディスクに対してスナップショットを作成します。
    3. 作成したスナップショットのSAS URIを取得し、一時的なアクセス権を付与します。
    4. スナップショットを指定されたBlob Storageにコピーします。
    5. スナップショットのSAS URIのアクセス権を取り消します。
    6. 最後にスナップショットを削除し、クリーンアップを行います。

    各ステップの実行時間は `Measure-ExecutionTime` を用いて計測され、操作名と共にログに記録されます。並列処理によって複数の仮想マシンに対して効率的にバックアップ処理を実行します。

.PARAMETER BackupName
    （オプション）バックアップの名前を指定します。指定しない場合、スクリプトが現在の日時を基に自動生成します。

.NOTES
    スクリプトは、共通関数を含む外部スクリプト `Common.ps1` をインポートして使用します。また、スクリプト内で使用されるリソースグループやストレージアカウントの名前は事前に設定されている必要があります。

.EXAMPLE
    .\Backup-VM.ps1 -BackupName "MyBackup"

    "MyBackup" という名前で仮想マシンのバックアップを作成します。

.EXAMPLE
    .\Backup-VM.ps1

    現在の日時を基にバックアップ名を自動生成し、仮想マシンのバックアップを作成します。
#>

param (
    [string] $BackupName
)

# 共通の関数や設定を含むスクリプトをインポート
. $PSScriptRoot\Common\Common.ps1  

# $BackupNameが指定されていない場合、現在の時刻を取得し、フォーマットされた名前を生成
if (-not $BackupName) {
  $BackupName = Get-Date -Format "yyyy.MM.dd_HH.mm.ss"
}

# バックアップ処理の実行時間を計測しながら処理を実行
Measure-ExecutionTime -operationName "'$BackupName' のバックアップ" -operation {
  $VirtualMachineNames | ForEach-Object -Parallel {
    # 並列実行のため、必要な関数を再度インポート
    . $using:PSScriptRoot\Common\Common.ps1  
  
    # 現在の仮想マシン名を設定
    $virtualMachineName = $_
    # 仮想マシンに対応するディスク名を取得
    $diskName = Get-DiskName -VirtualMachineName $virtualMachineName
    # スナップショットとVHDファイルの名前を生成
    $snapshotName = "$diskName-$using:BackupName"
    $vhdFileName = "$using:BackupName/$disName.vhd"
    
    # 1/5: スナップショットを作成し、その実行時間を計測
    Measure-ExecutionTime -operationName "'$virtualMachineName' [1/5] スナップショットの作成" -operation {
      $snapshotConfig = New-AzSnapshotConfig -SourceUri (Get-AzDisk -ResourceGroup $MyResourceGroup -DiskName $diskName).Id -Location "japaneast" -CreateOption Copy
      New-AzSnapshot -ResourceGroup $MyResourceGroup -SnapshotName $snapshotName -Snapshot $snapshotConfig > $null
    }
    
    try {
      # 2/5: スナップショットへのSAS URIのアクセスを許可し、その実行時間を計測
      $sasUri = Measure-ExecutionTime -operationName "'$virtualMachineName' [2/5] スナップショットのエクスポート" -operation {
        Grant-AzSnapshotAccess -ResourceGroup $MyResourceGroup -SnapshotName $snapshotName -Access Read -DurationInSecond 300
      }
    
      try {
        # 3/5: スナップショットをBlob Storageにコピーし、その実行時間を計測
        $storageContext = (Get-AzStorageAccount -ResourceGroup $ProductResourceGroup -Name $StorageAccountName).Context
        Measure-ExecutionTime -operationName "'$virtualMachineName' [3/5] スナップショットのコピー" -operation {
          Start-AzStorageBlobCopy -AbsoluteUri $sasUri.AccessSAS -DestContainer $ContainerName -DestBlob $vhdFileName -DestContext $storageContext -Force > $null
        }
      }
      finally {
        # 4/5: スナップショットのSAS URI のアクセスを取り消し、その実行時間を計測
        Measure-ExecutionTime -operationName "'$virtualMachineName' [4/5] スナップショットのSAS URI の取消" -operation {
          Revoke-AzSnapshotAccess -ResourceGroup $MyResourceGroup -SnapshotName $snapshotName > $null
        }
      }
    }
    finally {
      # 5/5: スナップショットを削除し、その実行時間を計測
      Measure-ExecutionTime -operationName "'$virtualMachineName' [5/5] スナップショットの削除" -operation {
        Remove-AzSnapshot -ResourceGroup $MyResourceGroup -SnapshotName $snapshotName -Force > $null
      }
    }
    
    # バックアップが完了したことをコンソールに表示
    Write-Host "'$virtualMachineName' を '$vhdFileName' にバックアップしました。" -ForegroundColor Cyan
  } -ThrottleLimit 4
}
