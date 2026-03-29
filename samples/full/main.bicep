targetScope = 'local'

@secure()
param netboxToken string

param netboxUrl string

extension netbox with {
  url: netboxUrl
  token: netboxToken
}

// Tenancy
resource tenant 'Tenant' = {
  name: 'Contoso'
  description: 'Contoso Corporation'
}

// DCIM - Site
resource site 'Site' = {
  name: 'Stockholm DC1'
  status: 'active'
  description: 'Primary datacenter in Stockholm'
}

// DCIM - Hardware catalog
resource manufacturer 'Manufacturer' = {
  name: 'Cisco'
  description: 'Cisco Systems'
}

resource deviceRole 'DeviceRole' = {
  name: 'Router'
  color: 'aa1409'
}

// IPAM
resource prefix 'Prefix' = {
  prefix: '10.100.0.0/24'
  status: 'active'
  description: 'Stockholm DC1 management network'
}

resource ipAddress 'IPAddress' = {
  address: '10.100.0.1/24'
  status: 'active'
  dnsName: 'gw-stockholm-dc1.contoso.com'
  description: 'Default gateway Stockholm DC1'
}

resource vlan 'VLAN' = {
  vid: 100
  name: 'Management'
  status: 'active'
  description: 'Management VLAN'
}
