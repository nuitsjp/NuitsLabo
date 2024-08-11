param networkInterfaces_arm_template288_name string = 'arm-template288'
param virtualNetworks_ARM_TEMPLATE_vnet_name string = 'ARM-TEMPLATE-vnet'
param virtualNetworks_ARM_TEMPLATE_vnet_rg string = 'rg-arm-template-study-dev-eastjp-001'
param networkSecurityGroups_ARMTEMPLATEnsg264_externalid string = '/subscriptions/fc7753ed-2e69-4202-bb66-86ff5798b8d5/resourceGroups/rg-arm-template-study-dev-eastjp-001/providers/Microsoft.Network/networkSecurityGroups/ARMTEMPLATEnsg264'

resource networkInterfaces_arm_template288_name_resource 'Microsoft.Network/networkInterfaces@2023-11-01' = {
  name: networkInterfaces_arm_template288_name
  location: 'japaneast'
  properties: {
    ipConfigurations: [
      {
        name: 'ipconfig1'
        id: resourceId('Microsoft.Network/networkInterfaces/ipConfigurations', networkInterfaces_arm_template288_name, 'ipconfig1')
        type: 'Microsoft.Network/networkInterfaces/ipConfigurations'
        properties: {
          privateIPAddress: '10.1.0.4'
          privateIPAllocationMethod: 'Dynamic'
          subnet: {
            id: resourceId(subscription().subscriptionId, virtualNetworks_ARM_TEMPLATE_vnet_rg, 'Microsoft.Network/virtualNetworks/subnets', virtualNetworks_ARM_TEMPLATE_vnet_name, 'default')
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
      id: networkSecurityGroups_ARMTEMPLATEnsg264_externalid
    }
    nicType: 'Standard'
    auxiliaryMode: 'None'
    auxiliarySku: 'None'
  }
}
