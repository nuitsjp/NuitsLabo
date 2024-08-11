param diskName string
param location string
param sku string
param diskSizeGb int
param sourceResourceId string
param createOption string
param diskEncryptionSetType string
param dataAccessAuthMode string
param networkAccessPolicy string
param publicNetworkAccess string
param networkInterfaceName string
param networkSecurityGroupId string
param subnetName string
param virtualNetworkId string
param virtualMachineName string
param virtualMachineComputerName string
param virtualMachineRG string
param osDiskDeleteOption string
param virtualMachineSize string
param nicDeleteOption string
param hibernationEnabled bool
param securityType string
param secureBoot bool
param vTPM bool
param autoShutdownStatus string
param autoShutdownTime string
param autoShutdownTimeZone string
param autoShutdownNotificationStatus string
param autoShutdownNotificationLocale string
param autoShutdownNotificationEmail string

var nsgId = networkSecurityGroupId
var vnetId = virtualNetworkId
var vnetName = last(split(vnetId, '/'))
var subnetRef = '${vnetId}/subnets/${subnetName}'

resource disk 'Microsoft.Compute/disks@2022-03-02' = {
  name: diskName
  location: location
  properties: {
    creationData: {
      createOption: createOption
      sourceResourceId: sourceResourceId
    }
    diskSizeGB: diskSizeGb
    encryption: {
      type: diskEncryptionSetType
    }
    dataAccessAuthMode: dataAccessAuthMode
    networkAccessPolicy: networkAccessPolicy
    publicNetworkAccess: publicNetworkAccess
  }
  sku: {
    name: sku
  }
  tags: {}
}

resource networkInterface 'Microsoft.Network/networkInterfaces@2022-11-01' = {
  name: networkInterfaceName
  location: location
  properties: {
    ipConfigurations: [
      {
        name: 'ipconfig1'
        properties: {
          subnet: {
            id: subnetRef
          }
          privateIPAllocationMethod: 'Dynamic'
        }
      }
    ]
    networkSecurityGroup: {
      id: nsgId
    }
  }
  dependsOn: []
}

resource virtualMachine 'Microsoft.Compute/virtualMachines@2024-03-01' = {
  name: virtualMachineName
  location: location
  properties: {
    hardwareProfile: {
      vmSize: virtualMachineSize
    }
    storageProfile: {
      osDisk: {
        createOption: 'attach'
        osType: 'Windows'
        managedDisk: {
          id: disk.id
        }
        deleteOption: osDiskDeleteOption
      }
    }
    networkProfile: {
      networkInterfaces: [
        {
          id: networkInterface.id
          properties: {
            deleteOption: nicDeleteOption
          }
        }
      ]
    }
    securityProfile: {
      securityType: securityType
      uefiSettings: {
        secureBootEnabled: secureBoot
        vTpmEnabled: vTPM
      }
    }
    additionalCapabilities: {
      hibernationEnabled: false
    }
    licenseType: 'Windows_Server'
    diagnosticsProfile: {
      bootDiagnostics: {
        enabled: true
      }
    }
  }
}

resource shutdown_computevm_virtualMachine 'Microsoft.DevTestLab/schedules@2018-09-15' = {
  name: 'shutdown-computevm-${virtualMachineName}'
  location: location
  properties: {
    status: autoShutdownStatus
    taskType: 'ComputeVmShutdownTask'
    dailyRecurrence: {
      time: autoShutdownTime
    }
    timeZoneId: autoShutdownTimeZone
    targetResourceId: virtualMachine.id
    notificationSettings: {
      status: autoShutdownNotificationStatus
      notificationLocale: autoShutdownNotificationLocale
      timeInMinutes: '30'
      emailRecipient: autoShutdownNotificationEmail
    }
  }
}
