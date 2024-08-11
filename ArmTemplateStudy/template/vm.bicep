param resourceGroupName string
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
param subscriptionId string  // サブスクリプションIDを追加

module diskMod 'diskModule.bicep' = {
  name: 'diskDeployment'
  scope: resourceGroup(resourceGroupName)
  params: {
    diskName: diskName
    location: location
    sku: sku
    diskSizeGb: diskSizeGb
    sourceResourceId: sourceResourceId
    createOption: createOption
    diskEncryptionSetType: diskEncryptionSetType
    dataAccessAuthMode: dataAccessAuthMode
    networkAccessPolicy: networkAccessPolicy
    publicNetworkAccess: publicNetworkAccess
  }
}

module networkInterfaceMod 'networkInterfaceModule.bicep' = {
  name: 'networkInterfaceDeployment'
  scope: resourceGroup(resourceGroupName)
  params: {
    networkInterfaceName: networkInterfaceName
    location: location
    networkSecurityGroupId: networkSecurityGroupId
    subnetName: subnetName
    virtualNetworkId: virtualNetworkId
  }
}

module virtualMachineMod 'virtualMachineModule.bicep' = {
  name: 'virtualMachineDeployment'
  scope: resourceGroup(resourceGroupName)
  params: {
    virtualMachineName: virtualMachineName
    location: location
    virtualMachineSize: virtualMachineSize
    osDiskDeleteOption: osDiskDeleteOption
    nicDeleteOption: nicDeleteOption
    hibernationEnabled: hibernationEnabled
    securityType: securityType
    secureBoot: secureBoot
    vTPM: vTPM
    diskId: diskMod.outputs.diskId
    networkInterfaceId: networkInterfaceMod.outputs.networkInterfaceId
  }
}

module shutdownMod 'shutdownModule.bicep' = {
  name: 'shutdownDeployment'
  scope: resourceGroup(resourceGroupName)
  params: {
    virtualMachineName: virtualMachineName
    location: location
    autoShutdownStatus: autoShutdownStatus
    autoShutdownTime: autoShutdownTime
    autoShutdownTimeZone: autoShutdownTimeZone
    autoShutdownNotificationStatus: autoShutdownNotificationStatus
    autoShutdownNotificationLocale: autoShutdownNotificationLocale
    autoShutdownNotificationEmail: autoShutdownNotificationEmail
    subscriptionId: subscriptionId  // サブスクリプションIDをモジュールに渡す
  }
}
