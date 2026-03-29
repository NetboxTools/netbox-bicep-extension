targetScope = 'local'

@secure()
param netboxToken string

param netboxUrl string

extension netbox with {
  url: netboxUrl
  token: netboxToken
}

resource site 'Site' = {
  name: 'My Datacenter'
  status: 'active'
  description: 'Primary datacenter managed via Bicep'
  physicalAddress: '123 Server Lane, Cloud City'
}
