targetScope = 'local'

@secure()
param netboxToken string

param netboxUrl string

@description('The Azure public IP address to register in NetBox (e.g. "20.1.2.3/32").')
param azurePublicIp string

extension netbox with {
  url: netboxUrl
  token: netboxToken
}

resource ipAddress 'IPAddress' = {
  address: azurePublicIp
  status: 'active'
  description: 'Azure Public IP - registered via Bicep extension'
  dnsName: 'pip-netbox-demo.northeurope.cloudapp.azure.com'
}
