param virtualMachineName string
param location string
param virtualMachineSize string
param diskId string
param osDiskDeleteOption string
param nicId string
param nicDeleteOption string
param hibernationEnabled bool
param securityType string
param secureBoot bool
param vTPM bool

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
          id: diskId
        }
        deleteOption: osDiskDeleteOption
      }
    }
    networkProfile: {
      networkInterfaces: [
        {
          id: nicId
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
      hibernationEnabled: hibernationEnabled
    }
    licenseType: 'Windows_Server'
    diagnosticsProfile: {
      bootDiagnostics: {
        enabled: true
      }
    }
  }
}

output vmId string = virtualMachine.id
