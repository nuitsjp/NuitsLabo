Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"
$ProgressPreference = 'SilentlyContinue'

Write-Output "Uninstalling Claude Code..."
Write-Output ""

# Claude Codeの主要なパスを定義
$CLAUDE_DIR = "$env:USERPROFILE\.claude"
$DOWNLOADS_DIR = "$CLAUDE_DIR\downloads"
$BIN_DIR = "$CLAUDE_DIR\bin"
$DATA_DIR = "$env:LOCALAPPDATA\Claude"
$APPDATA_DIR = "$env:APPDATA\Claude"

# プロセスが実行中でないか確認
$claudeProcesses = Get-Process -Name "claude*" -ErrorAction SilentlyContinue
if ($claudeProcesses) {
    Write-Output "Stopping Claude Code processes..."
    $claudeProcesses | Stop-Process -Force
    Start-Sleep -Seconds 2
}

# PATHからClaude binディレクトリを削除
Write-Output "Removing Claude from PATH..."
$userPath = [Environment]::GetEnvironmentVariable("Path", "User")
if ($userPath) {
    $paths = $userPath -split ';' | Where-Object { $_ -and $_ -notlike "*\.claude\bin*" }
    $newPath = $paths -join ';'
    [Environment]::SetEnvironmentVariable("Path", $newPath, "User")
}

# Windowsのスタートメニューショートカットを削除
$startMenuPaths = @(
    "$env:APPDATA\Microsoft\Windows\Start Menu\Programs\Claude*.lnk",
    "$env:ProgramData\Microsoft\Windows\Start Menu\Programs\Claude*.lnk"
)
foreach ($path in $startMenuPaths) {
    if (Test-Path $path) {
        Write-Output "Removing Start Menu shortcuts..."
        Remove-Item -Path $path -Force -ErrorAction SilentlyContinue
    }
}

# デスクトップショートカットを削除
$desktopPaths = @(
    "$env:USERPROFILE\Desktop\Claude*.lnk",
    "$env:PUBLIC\Desktop\Claude*.lnk"
)
foreach ($path in $desktopPaths) {
    if (Test-Path $path) {
        Write-Output "Removing Desktop shortcuts..."
        Remove-Item -Path $path -Force -ErrorAction SilentlyContinue
    }
}

# レジストリエントリを削除（コンテキストメニューなど）
Write-Output "Removing registry entries..."
$registryPaths = @(
    "HKCU:\Software\Classes\*\shell\Open with Claude Code",
    "HKCU:\Software\Classes\Directory\shell\Open with Claude Code",
    "HKCU:\Software\Classes\Directory\Background\shell\Open with Claude Code",
    "HKCU:\Software\Claude"
)
foreach ($regPath in $registryPaths) {
    if (Test-Path $regPath) {
        Remove-Item -Path $regPath -Recurse -Force -ErrorAction SilentlyContinue
    }
}

# Claude関連のディレクトリを削除
$dirsToRemove = @($CLAUDE_DIR, $DATA_DIR, $APPDATA_DIR)
foreach ($dir in $dirsToRemove) {
    if (Test-Path $dir) {
        Write-Output "Removing directory: $dir"
        try {
            Remove-Item -Path $dir -Recurse -Force -ErrorAction Stop
        }
        catch {
            Write-Warning "Could not remove directory: $dir. Error: $_"
        }
    }
}

# 環境変数の更新を現在のセッションに反映
$env:Path = [Environment]::GetEnvironmentVariable("Path", "User")

Write-Output ""
Write-Output "$([char]0x2705) Claude Code has been successfully uninstalled!"
Write-Output ""
Write-Output "You may need to restart your terminal or computer for all changes to take effect."
