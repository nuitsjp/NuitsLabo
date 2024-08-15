param diskId string
param location string
param networkInterfaceName string
param networkSubscriptionId string
param networkResourceGroupName string
param networkSecurityGroupName string
param subnetName string
param virtualNetworkName string
param virtualMachineName string
param virtualMachineSize string
param osDiskDeleteOption string
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

module networkInterfaceModule 'module/networkInterface.bicep' = {
  name: 'networkInterfaceModule'
  params: {
    networkInterfaceName: networkInterfaceName
    location: location
    networkSubscriptionId: networkSubscriptionId
    networkResourceGroupName: networkResourceGroupName
    networkSecurityGroupName: networkSecurityGroupName
    subnetName: subnetName
    virtualNetworkName: virtualNetworkName
  }
}

module virtualMachineModule 'module/virtualMachine.bicep' = {
  name: 'virtualMachineModule'
  params: {
    virtualMachineName: virtualMachineName
    location: location
    virtualMachineSize: virtualMachineSize
    diskId: diskId
    osDiskDeleteOption: osDiskDeleteOption
    nicId: networkInterfaceModule.outputs.nicId
    nicDeleteOption: nicDeleteOption
    hibernationEnabled: hibernationEnabled
    securityType: securityType
    secureBoot: secureBoot
    vTPM: vTPM
  }
}

module shutdownModule 'module/shutdown.bicep' = {
  name: 'shutdownModule'
  params: {
    virtualMachineName: virtualMachineName
    location: location
    autoShutdownStatus: autoShutdownStatus
    autoShutdownTime: autoShutdownTime
    autoShutdownTimeZone: autoShutdownTimeZone
    autoShutdownNotificationStatus: autoShutdownNotificationStatus
    autoShutdownNotificationLocale: autoShutdownNotificationLocale
    autoShutdownNotificationEmail: autoShutdownNotificationEmail
    virtualMachineId: virtualMachineModule.outputs.vmId
  }
}
