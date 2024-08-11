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
