using 'netbox.bicep'

param netboxUrl = readEnvironmentVariable('NETBOX_URL')
param netboxToken = readEnvironmentVariable('NETBOX_TOKEN')
param azurePublicIp = readEnvironmentVariable('AZURE_PUBLIC_IP')
