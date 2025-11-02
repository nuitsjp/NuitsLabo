$DriveLetter = "S:"

Remove-ItemProperty -Path "HKLM:\SYSTEM\CurrentControlSet\Control\Session Manager\DOS Devices" -Name $DriveLetter -Force