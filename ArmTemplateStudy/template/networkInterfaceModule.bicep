param networkInterfaceName string
param location string
param networkSecurityGroupId string
param subnetName string
param virtualNetworkId string

var subnetRef = '${virtualNetworkId}/subnets/${subnetName}'

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
      id: networkSecurityGroupId
    }
  }
  dependsOn: []
}

output networkInterfaceId string = networkInterface.id
