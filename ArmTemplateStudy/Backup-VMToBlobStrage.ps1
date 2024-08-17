<#
.SYNOPSIS
    仮想マシンのディスクをスナップショットとして取得し、それをBlob StorageにバックアップするPowerShellスクリプト。

.DESCRIPTION
    このスクリプトは、指定された仮想マシンのディスクをスナップショットとして作成し、そのスナップショットをAzure Blob Storageにバックアップする一連の処理を行います。スクリプトは以下のステップで動作します。

    1. Version, Issue, BackupNameのいずれか1つを指定することで、バックアップ名を決定します。
       - Versionは文字列で指定され、その先頭に「V」を付与してBackupNameとします。
       - Issueは数値で指定され、その先頭に「Issue-」を付与してBackupNameとします（Issue番号は4桁のゼロ埋め）。
       - BackupNameが指定された場合は、そのままBackupNameとして使用します。
    2. 指定された仮想マシンに対して並列処理を行い、それぞれのディスクに対してスナップショットを作成します。
    3. 作成したスナップショットのSAS URIを取得し、一時的なアクセス権を付与します。
    4. スナップショットを指定されたBlob Storageにコピーします。
    5. スナップショットのSAS URIのアクセス権を取り消します。
    6. 最後にスナップショットを削除し、クリーンアップを行います。

    各ステップの実行時間は `Measure-ExecutionTime` を用いて計測され、操作名と共にログに記録されます。並列処理によって複数の仮想マシンに対して効率的にバックアップ処理を実行します。

.PARAMETER Version
    （オプション）バージョン名を指定します。指定された文字列の先頭に「V」を付与してBackupNameを生成します。

.PARAMETER Issue
    （オプション）Issue番号を指定します。指定された番号の先頭に「Issue-」を付与してBackupNameを生成します。Issue番号は4桁のゼロ埋めを行います。

.PARAMETER BackupName
    （オプション）バックアップの名前を直接指定します。

.NOTES
    Version, Issue, BackupNameのいずれか1つだけを指定してください。複数指定や未指定の場合はエラーとなります。

.EXAMPLE
    .\Backup-VM.ps1 -Version "1.0.0"

    "V1.0.0" という名前で仮想マシンのバックアップを作成します。

.EXAMPLE
    .\Backup-VM.ps1 -Issue 123

    "Issue-0123" という名前で仮想マシンのバックアップを作成します。

.EXAMPLE
    .\Backup-VM.ps1 -BackupName "MyBackup"

    "MyBackup" という名前で仮想マシンのバックアップを作成します。
#>

param (
  [string] $Version,
  [int] $Issue,
  [string] $BackupName
)

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

# 共通の関数や設定を含むスクリプトをインポート
. $PSScriptRoot\Common\Common.ps1  

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
    $snapshotName = "$diskName-backup-working"
    $vhdFileName = "$using:BackupName/$($diskName).vhd"
    
    # 1/5: スナップショットを作成し、その実行時間を計測
    Measure-ExecutionTime -operationName "'$virtualMachineName' [1/5] スナップショットの作成" -operation {
      $snapshotConfig = New-AzSnapshotConfig -SourceUri (Get-AzDisk -ResourceGroup $MyResourceGroup -DiskName $diskName).Id -Location "japaneast" -CreateOption Copy
      New-AzSnapshot -ResourceGroup $MyResourceGroup -SnapshotName $snapshotName -Snapshot $snapshotConfig > $null
    }
    
    try {
      # Provide Shared Access Signature (SAS) expiry duration in seconds e.g. 3600.
      # Know more about SAS here: https://docs.microsoft.com/azure/storage/storage-dotnet-shared-access-signature-part-1
      $sasExpiryDuration = 3600

          # 2/5: スナップショットへのSAS URIのアクセスを許可し、その実行時間を計測
      $sas = Measure-ExecutionTime -operationName "'$virtualMachineName' [2/5] スナップショットのエクスポート" -operation {
        Grant-AzSnapshotAccess -ResourceGroup $MyResourceGroup -SnapshotName $snapshotName -Access Read -DurationInSecond $sasExpiryDuration
      }
    
      try {
        # 3/5: スナップショットをBlob Storageにコピーし、その実行時間を計測
        Measure-ExecutionTime -operationName "'$virtualMachineName' [3/5] スナップショットのコピー" -operation {

          $destinationContext = (Get-AzStorageAccount -ResourceGroup $ProductResourceGroup -Name $StorageAccountName).Context
          $containerSASURI = New-AzStorageBlobSASToken `
            -Context $destinationContext `
            -Container $ContainerName `
            -Blob $vhdFileName `
            -Permission racwd `
            -ExpiryTime (Get-Date).AddSeconds($sasExpiryDuration) `
            -FullUri
          azcopy copy $sas.AccessSAS $containerSASURI > $null
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
