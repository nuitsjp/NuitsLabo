# 読み取り専用変数（定数）を定義
Set-Variable -Name SubscriptionId -Value "fc7753ed-2e69-4202-bb66-86ff5798b8d5" -Option ReadOnly -Scope Script
Set-Variable -Name ResourceGroup -Value "rg-arm-template-study-dev-eastjp-001" -Option ReadOnly -Scope Script
Set-Variable -Name Location -Value "japaneast" -Option ReadOnly -Scope Script
Set-Variable -Name DiskName -Value "osdisk-arm-template-study-dev-japaneast-001" -Option ReadOnly -Scope Script
Set-Variable -Name DiskSku -Value "Standard_LRS" -Option ReadOnly -Scope Script

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
