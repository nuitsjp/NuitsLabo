$VerbosePreference = 'Continue'
$ErrorActionPreference = "Stop"

. .\Functions.ps1

try
{
    Get-WindowsServices `
        | ForEach-Object -Process {Stop-Service $_.Name}
}
catch
{
    Write-Exception $_.Exception
    exit 1
}