param disks_ARM_TEMPLATE_name string = 'ARM-TEMPLATE'
param snapshots_snp_arm_template_study_dev_eastjp_20240811073046_externalid string = '/subscriptions/fc7753ed-2e69-4202-bb66-86ff5798b8d5/resourceGroups/rg-arm-template-study-dev-eastjp-001/providers/Microsoft.Compute/snapshots/snp-arm-template-study-dev-eastjp-20240811073046'

resource disks_ARM_TEMPLATE_name_resource 'Microsoft.Compute/disks@2023-10-02' = {
  name: disks_ARM_TEMPLATE_name
  location: 'japaneast'
  sku: {
    name: 'Standard_LRS'
  }
  properties: {
    osType: 'Windows'
    hyperVGeneration: 'V2'
    supportsHibernation: true
    supportedCapabilities: {
      diskControllerTypes: 'SCSI, NVMe'
      acceleratedNetwork: true
      architecture: 'x64'
    }
    creationData: {
      createOption: 'Copy'
      sourceResourceId: snapshots_snp_arm_template_study_dev_eastjp_20240811073046_externalid
    }
    diskSizeGB: 127
    diskIOPSReadWrite: 500
    diskMBpsReadWrite: 60
    encryption: {
      type: 'EncryptionAtRestWithPlatformKey'
    }
    networkAccessPolicy: 'AllowAll'
    securityProfile: {
      securityType: 'TrustedLaunch'
    }
    publicNetworkAccess: 'Enabled'
    dataAccessAuthMode: 'None'
  }
}
