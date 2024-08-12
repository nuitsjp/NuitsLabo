param networkInterfaceName string
param location string
param networkSubscriptionId string
param networkResourceGroupName string
param networkSecurityGroupName string
param subnetName string
param virtualNetworkName string

resource networkInterface 'Microsoft.Network/networkInterfaces@2022-11-01' = {
  name: networkInterfaceName
  location: location
  properties: {
    ipConfigurations: [
      {
        name: 'ipconfig1'
        properties: {
          subnet: {
            id: resourceId(networkSubscriptionId, networkResourceGroupName, 'Microsoft.Network/virtualNetworks/subnets', virtualNetworkName, subnetName)
          }
          privateIPAllocationMethod: 'Dynamic'
        }
      }
    ]
    networkSecurityGroup: {
      id: resourceId(networkSubscriptionId, networkResourceGroupName, 'Microsoft.Network/networkSecurityGroups', networkSecurityGroupName)
    }
  }
}

output nicId string = networkInterface.id
