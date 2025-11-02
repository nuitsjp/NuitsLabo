param(
    [Parameter(Position = 0)]
    [string]$Source,

    [ValidateSet('html', 'pdf', 'pptx')]
    [string]$Format = 'html',

    [string]$Output,

    [switch]$Watch,

    [switch]$Serve,

    [switch]$Latest
)

$ErrorActionPreference = 'Stop'

# Ensure npm commands run from the project root so local dependencies resolve.
$projectRoot = Split-Path -Parent $PSScriptRoot

Push-Location -Path $projectRoot
try {
    if (-not $Serve -and -not $Source) {
        $Source = 'Sample.md'
    }

    $packageId = if ($Latest) { '@marp-team/marp-cli@latest' } else { '@marp-team/marp-cli' }
    $cmdArgs = @($packageId)

    if ($Serve) {
        if ($Output) {
            throw 'Output path cannot be specified when using -Serve.'
        }

        $serveTarget = if ($Source) { (Resolve-Path -Path $Source).Path } else { (Resolve-Path -Path '.').Path }
        $cmdArgs += '-s'
        $cmdArgs += $serveTarget
    }
    else {
        if (-not $Source) {
            throw 'Source markdown file is required unless -Serve is specified.'
        }

        $resolvedSource = (Resolve-Path -Path $Source).Path

        if ($Watch) {
            $cmdArgs += '-w'
        }

        $cmdArgs += $resolvedSource

        switch ($Format) {
            'pdf' { $cmdArgs += '--pdf' }
            'pptx' { $cmdArgs += '--pptx' }
            default { }
        }

        if ($Output) {
            $absoluteOutput = [System.IO.Path]::GetFullPath($Output, (Get-Location).Path)
            $cmdArgs += '-o'
            $cmdArgs += $absoluteOutput
        }
    }

    & npx @cmdArgs

    if ($LASTEXITCODE -ne 0) {
        throw "marp CLI exited with code $LASTEXITCODE."
    }
}
finally {
    Pop-Location
}
