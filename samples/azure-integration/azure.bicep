targetScope = 'resourceGroup'

@description('Azure region for the public IP resource.')
param location string = resourceGroup().location

@description('Name of the public IP address resource.')
param publicIpName string = 'pip-netbox-demo'

resource publicIp 'Microsoft.Network/publicIPAddresses@2024-05-01' = {
  name: publicIpName
  location: location
  sku: {
    name: 'Standard'
  }
  properties: {
    publicIPAllocationMethod: 'Static'
  }
}

@description('The allocated public IP address.')
output ipAddress string = publicIp.properties.ipAddress
