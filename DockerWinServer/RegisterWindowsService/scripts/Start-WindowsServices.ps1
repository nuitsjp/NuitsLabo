$VerbosePreference = 'Continue'
$ErrorActionPreference = "Stop"

. .\Functions.ps1

for ($i=0; $i -lt 10; $i++){
    try
    {
        Get-WindowsServices `
            | ForEach-Object -Process {Start-Service $_.Name}
        break
    }
    catch
    {
        Start-Sleep -Seconds 1
    }
}

pwsh.exe