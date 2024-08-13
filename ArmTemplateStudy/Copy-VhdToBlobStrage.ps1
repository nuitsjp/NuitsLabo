# サブスクリプションIDを指定
$subscriptionId = "fc7753ed-2e69-4202-bb66-86ff5798b8d5"

# マネージドディスクが作成されたリソースグループ名を指定
$resourceGroupName = "rg-arm-template-study-dev-japaneast-001"

# マネージドディスクの名前を指定
$diskName = "osdisk-vm-001-dev-japaneast-Issue001"

# Shared Access Signature (SAS) の有効期限を秒単位で指定（例: 3600秒）
# SASの詳細については、https://docs.microsoft.com/azure/storage/storage-dotnet-shared-access-signature-part-1 を参照
$sasExpiryDuration = 3600

# VHDファイルをコピーする対象のストレージアカウント名を指定
$storageAccountName = "armtemplatestudy"

# ダウンロードしたVHDが保存されるストレージコンテナの名前を指定
$storageContainerName = "disk-backup"

# マネージドディスクのVHDをコピーする先のVHDファイル名を指定
$destinationVHDFileName = "$diskName.vhd"

# サブスクリプションを設定
az account set --subscription $subscriptionId

# ストレージアカウントキーを取得
$storageAccountKey = az storage account keys list `
  --resource-group $resourceGroupName `
  --account-name $storageAccountName `
  --query "[0].value" `
  --output tsv


  # SAS URLを取得
$sas = "https://md-mbzkjn22cjqw.z26.blob.storage.azure.net/bfqllx4mpbx2/abcd?sv=2018-03-28&sr=b&si=a87795f7-287c-4f6a-b79b-aaf2202472de&sig=ibtLi6PQxl%2F7KcrMQ2INgciRaXouR3HFn45L3QLMnj8%3D"

# VHDファイルをコピー開始
# az storage blob copy start --destination-blob $destinationVHDFileName --destination-container $storageContainerName --account-name $storageAccountName --account-key $storageAccountKey --source-uri $sas
# AzCopyコマンドの実行 --blob-type PageBlob
azcopy copy "$sas" "https://armtemplatestudy.blob.core.windows.net/disk-backup?sp=rw&st=2024-08-13T01:17:28Z&se=2024-08-13T09:17:28Z&spr=https&sv=2022-11-02&sr=c&sig=o2PBE0f4XW8O7IaNFpG0Z5ylGrZze46HnupRYaz%2BEAc%3D" --blob-type PageBlob