param virtualMachineName string
param location string
param autoShutdownStatus string
param autoShutdownTime string
param autoShutdownTimeZone string
param autoShutdownNotificationStatus string
param autoShutdownNotificationLocale string
param autoShutdownNotificationEmail string
param subscriptionId string  // サブスクリプションIDをパラメータとして追加

resource shutdown_computevm_virtualMachine 'Microsoft.DevTestLab/schedules@2018-09-15' = {
  name: 'shutdown-computevm-${virtualMachineName}'
  location: location
  properties: {
    status: autoShutdownStatus
    taskType: 'ComputeVmShutdownTask'
    dailyRecurrence: {
      time: autoShutdownTime
    }
    timeZoneId: autoShutdownTimeZone
    targetResourceId: resourceId(subscriptionId, 'rg-arm-template-study-dev-eastjp-001', 'Microsoft.Compute/virtualMachines', virtualMachineName) 
    notificationSettings: {
      status: autoShutdownNotificationStatus
      notificationLocale: autoShutdownNotificationLocale
      timeInMinutes: 30
      emailRecipient: autoShutdownNotificationEmail
    }
  }
}
