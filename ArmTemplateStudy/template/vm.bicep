param virtualMachines_ARM_TEMPLATE_name string = 'ARM-TEMPLATE'
param disks_ARM_TEMPLATE_externalid string = '/subscriptions/fc7753ed-2e69-4202-bb66-86ff5798b8d5/resourceGroups/rg-arm-template-study-dev-eastjp-001/providers/Microsoft.Compute/disks/ARM-TEMPLATE'
param networkInterfaces_arm_template288_externalid string = '/subscriptions/fc7753ed-2e69-4202-bb66-86ff5798b8d5/resourceGroups/rg-arm-template-study-dev-eastjp-001/providers/Microsoft.Network/networkInterfaces/arm-template288'

resource virtualMachines_ARM_TEMPLATE_name_resource 'Microsoft.Compute/virtualMachines@2024-03-01' = {
  name: virtualMachines_ARM_TEMPLATE_name
  location: 'japaneast'
  properties: {
    hardwareProfile: {
      vmSize: 'Standard_D2s_v3'
    }
    additionalCapabilities: {
      hibernationEnabled: false
    }
    storageProfile: {
      osDisk: {
        osType: 'Windows'
        name: virtualMachines_ARM_TEMPLATE_name
        createOption: 'Attach'
        caching: 'ReadWrite'
        managedDisk: {
          storageAccountType: 'Standard_LRS'
          id: disks_ARM_TEMPLATE_externalid
        }
        deleteOption: 'Delete'
        diskSizeGB: 127
      }
      dataDisks: []
      diskControllerType: 'SCSI'
    }
    securityProfile: {
      uefiSettings: {
        secureBootEnabled: true
        vTpmEnabled: true
      }
      securityType: 'TrustedLaunch'
    }
    networkProfile: {
      networkInterfaces: [
        {
          id: networkInterfaces_arm_template288_externalid
          properties: {
            deleteOption: 'Delete'
          }
        }
      ]
    }
    diagnosticsProfile: {
      bootDiagnostics: {
        enabled: true
      }
    }
    licenseType: 'Windows_Server'
  }
}
