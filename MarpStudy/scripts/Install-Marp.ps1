$ErrorActionPreference = "Stop"

$projectRoot = Split-Path -Parent $PSScriptRoot

Push-Location -Path $projectRoot
try {
    npm ci
}
finally {
    Pop-Location
}
