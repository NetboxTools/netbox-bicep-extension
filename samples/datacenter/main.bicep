targetScope = 'local'

@secure()
param netboxToken string

param netboxUrl string

extension netbox with {
  url: netboxUrl
  token: netboxToken
}

// ─── Tenancy ───────────────────────────────────────

resource tenant 'Tenant' = {
  name: 'Contoso'
  slug: 'contoso'
  description: 'Contoso Corporation'
}

// ─── Sites ─────────────────────────────────────────

resource siteStockholm 'Site' = {
  name: 'Stockholm DC1'
  slug: 'stockholm-dc1'
  status: 'active'
  description: 'Primary datacenter in Stockholm'
  physicalAddress: 'Sveavägen 1, Stockholm'
  latitude: '59.3293'
  longitude: '18.0686'
  timeZone: 'Europe/Stockholm'
}

resource siteLondon 'Site' = {
  name: 'London DC1'
  slug: 'london-dc1'
  status: 'planned'
  description: 'Secondary datacenter in London'
  physicalAddress: '1 Data Centre Rd, London'
  latitude: '51.5074'
  longitude: '-0.1278'
  timeZone: 'Europe/London'
}

// ─── Hardware catalog ──────────────────────────────

resource cisco 'Manufacturer' = {
  name: 'Cisco'
  slug: 'cisco'
  description: 'Cisco Systems, Inc.'
}

resource juniper 'Manufacturer' = {
  name: 'Juniper'
  slug: 'juniper'
  description: 'Juniper Networks'
}

resource dell 'Manufacturer' = {
  name: 'Dell'
  slug: 'dell'
  description: 'Dell Technologies'
}

// ─── Device Roles ──────────────────────────────────

resource roleRouter 'DeviceRole' = {
  name: 'Router'
  slug: 'router'
  color: 'aa1409'
  description: 'Core and edge routers'
}

resource roleSwitch 'DeviceRole' = {
  name: 'Switch'
  slug: 'switch'
  color: '2196f3'
  description: 'Access and distribution switches'
}

resource roleFirewall 'DeviceRole' = {
  name: 'Firewall'
  slug: 'firewall'
  color: 'ff9800'
  description: 'Network firewalls'
}

resource roleServer 'DeviceRole' = {
  name: 'Server'
  slug: 'server'
  color: '4caf50'
  description: 'Physical servers'
}
