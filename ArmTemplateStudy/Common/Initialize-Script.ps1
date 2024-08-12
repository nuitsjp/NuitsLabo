$ErrorActionPreference = "Stop"

. $PSScriptRoot\Common.ps1

# 現在のサブスクリプションを確認
Write-Host "サブスクリプションの確認中..."

# 現在のサブスクリプションが希望のものと異なる場合、切り替える
$currentSubscriptionId = az account show --query id -o tsv
if ($currentSubscriptionId -ne $SubscriptionId) {
    Write-Output "サブスクリプションを '$SubscriptionId' に切り替えています..."
    az account set --subscription $SubscriptionId
    if ($LASTEXITCODE -eq 0) {
        Write-Output "サブスクリプションの切り替えに成功しました。"
    } else {
        Write-Error "サブスクリプションの切り替えに失敗しました。"
        exit 1
    }
}
