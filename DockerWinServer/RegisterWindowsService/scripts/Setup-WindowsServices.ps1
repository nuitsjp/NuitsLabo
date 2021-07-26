$VerbosePreference = 'Continue'
$ErrorActionPreference = "Stop"

. .\Functions.ps1

try
{
        Get-Service -Name "Simple*" `
                | ForEach-Object -Process {Stop-Service -Name $_.Name}
        Get-Service -Name "Simple*" `
                | ForEach-Object -Process {Remove-Service -Name $_.Name}

        Get-WindowsServices `
                | ForEach-Object -Process {Register-WindowsService $_}
}
catch
{
    Write-Exception $_.Exception
    exit 1
}
