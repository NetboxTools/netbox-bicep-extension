using 'main.bicep'

param netboxUrl = readEnvironmentVariable('NETBOX_URL')
param netboxToken = readEnvironmentVariable('NETBOX_TOKEN')
param defaultPassword = readEnvironmentVariable('NETBOX_DEFAULT_PASSWORD')
