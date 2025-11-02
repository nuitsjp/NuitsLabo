function New-SystemDriveMapping {
    param(
        [Parameter(Mandatory=$true)]
        [string]$DriveLetter,
        [Parameter(Mandatory=$true)]
        [string]$TargetPath
    )
    
    $regPath = "HKLM:\SYSTEM\CurrentControlSet\Control\Session Manager\DOS Devices"
    $dosPath = "\??\$TargetPath"
    
    # 既存の登録を確認して削除
    if (Get-ItemProperty -Path $regPath -Name $DriveLetter -ErrorAction SilentlyContinue) {
        Remove-ItemProperty -Path $regPath -Name $DriveLetter -Force
    }
    
    # 新規登録
    New-ItemProperty -Path $regPath -Name $DriveLetter -Value $dosPath -PropertyType String -Force
    
    Write-Host "ドライブマッピング完了: $DriveLetter -> $TargetPath"
    Write-Host "システム再起動後に有効になります"
}

New-SystemDriveMapping -DriveLetter "S:" -TargetPath "C:\Work\DosDriveS"
