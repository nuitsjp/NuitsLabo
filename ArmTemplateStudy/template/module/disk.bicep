param diskName string
param location string
param sku string
param diskSizeGb int
param snapshotId string
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
      sourceUri: snapshotId
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
}

output diskId string = disk.id
