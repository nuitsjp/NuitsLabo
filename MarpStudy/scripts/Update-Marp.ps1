$ErrorActionPreference = "Stop"

$projectRoot = Split-Path -Parent $PSScriptRoot

Push-Location -Path $projectRoot
try {
    npm update --save-dev @marp-team/marp-cli
}
finally {
    Pop-Location
}
