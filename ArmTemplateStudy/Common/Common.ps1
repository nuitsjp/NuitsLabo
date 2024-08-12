function Get-SnapshotPrefix {
    param (
        [Parameter(Mandatory = $true)]
        [string] $VirtualMachineName
    )
    return "snp-$VirtualMachineName-dev-japaneast"
}

function Get-DiskName {
    param (
        [Parameter(Mandatory = $true)]
        [string] $VirtualMachineName
    )
    return "osdisk-$VirtualMachineName-dev-japaneast"
}

function Get-NicName {
    param (
        [Parameter(Mandatory = $true)]
        [string] $VirtualMachineName
    )
    return "nic-$VirtualMachineName-dev-japaneast"
}

# 読み取り専用変数（定数）を定義
Set-Variable -Name SubscriptionId -Value "fc7753ed-2e69-4202-bb66-86ff5798b8d5" -Option ReadOnly -Scope Script
Set-Variable -Name CompanyResourceGroup -Value "rg-arm-template-study-dev-japaneast-company" -Option ReadOnly -Scope Script
Set-Variable -Name ProductResourceGroup -Value "rg-arm-template-study-dev-japaneast-product" -Option ReadOnly -Scope Script
Set-Variable -Name MyResourceGroup -Value "rg-arm-template-study-dev-japaneast-001" -Option ReadOnly -Scope Script
Set-Variable -Name Location -Value "japaneast" -Option ReadOnly -Scope Script
Set-Variable -Name VirtualMachineNames -Value @("vm-001", "vm-002") -Option ReadOnly -Scope Script
