param networkInterface_name string = 'nic-arm-template-study-dev-japaneast-001'
param vnet_name string = 'ARM-TEMPLATE-vnet'
param vnet_resourceGroupName string = 'rg-arm-template-study-dev-japaneast-001'
param networkSecurityGroups_externalid string = '/subscriptions/fc7753ed-2e69-4202-bb66-86ff5798b8d5/resourceGroups/rg-arm-template-study-dev-japaneast-001/providers/Microsoft.Network/networkSecurityGroups/ARMTEMPLATEnsg264'

resource networkInterface_resource 'Microsoft.Network/networkInterfaces@2023-11-01' = {
  name: networkInterface_name
  location: 'japaneast'
  properties: {
    ipConfigurations: [
      {
        name: 'ipconfig1'
        id: resourceId('Microsoft.Network/networkInterfaces/ipConfigurations', networkInterface_name, 'ipconfig1')
        type: 'Microsoft.Network/networkInterfaces/ipConfigurations'
        properties: {
          privateIPAddress: '10.1.0.4'
          privateIPAllocationMethod: 'Dynamic'
          subnet: {
            id: resourceId(subscription().subscriptionId, vnet_resourceGroupName, 'Microsoft.Network/virtualNetworks/subnets', vnet_name, 'default')
          }
          primary: true
          privateIPAddressVersion: 'IPv4'
        }
      }
    ]
    dnsSettings: {
      dnsServers: []
    }
    enableAcceleratedNetworking: false
    enableIPForwarding: false
    disableTcpStateTracking: false
    networkSecurityGroup: {
      id: networkSecurityGroups_externalid
    }
    nicType: 'Standard'
    auxiliaryMode: 'None'
    auxiliarySku: 'None'
  }
}
