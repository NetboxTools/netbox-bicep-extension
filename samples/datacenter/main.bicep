targetScope = 'local'

@secure()
param netboxToken string

param netboxUrl string

extension netbox with {
  url: netboxUrl
  token: netboxToken
}

// ─── Regions ──────────────────────────────────────

resource regionFrance 'Region' = {
  name: 'France'
  description: 'France'
}

resource regionSweden 'Region' = {
  name: 'Sweden'
  description: 'Sweden'
}

resource regionJapan 'Region' = {
  name: 'Japan'
  description: 'Japan'
}

resource regionUS 'Region' = {
  name: 'United States'
  description: 'United States'
}

resource regionChina 'Region' = {
  name: 'China'
  description: 'China'
}

// ─── Tenancy ───────────────────────────────────────

resource tenant 'Tenant' = {
  name: 'Contoso'
  description: 'Contoso Corporation'
}

// ─── Sites (capital city per region) ──────────────

resource siteParis 'Site' = {
  name: 'Paris DC1'
  status: 'active'
  region: regionFrance.id
  description: 'Datacenter in Paris'
  physicalAddress: 'Rue de Rivoli, Paris'
  latitude: '48.8566'
  longitude: '2.3522'
  timeZone: 'Europe/Paris'
}

resource siteStockholm 'Site' = {
  name: 'Stockholm DC1'
  status: 'active'
  region: regionSweden.id
  description: 'Primary datacenter in Stockholm'
  physicalAddress: 'Sveavägen 1, Stockholm'
  latitude: '59.3293'
  longitude: '18.0686'
  timeZone: 'Europe/Stockholm'
}

resource siteTokyo 'Site' = {
  name: 'Tokyo DC1'
  status: 'active'
  region: regionJapan.id
  description: 'Datacenter in Tokyo'
  physicalAddress: 'Chiyoda, Tokyo'
  latitude: '35.6762'
  longitude: '139.6503'
  timeZone: 'Asia/Tokyo'
}

resource siteWashington 'Site' = {
  name: 'Washington DC1'
  status: 'active'
  region: regionUS.id
  description: 'Datacenter in Washington D.C.'
  physicalAddress: '1600 Pennsylvania Ave, Washington'
  latitude: '38.8977'
  longitude: '-77.0365'
  timeZone: 'America/New_York'
}

resource siteBeijing 'Site' = {
  name: 'Beijing DC1'
  status: 'active'
  region: regionChina.id
  description: 'Datacenter in Beijing'
  physicalAddress: 'Zhongguancun, Beijing'
  latitude: '39.9042'
  longitude: '116.4074'
  timeZone: 'Asia/Shanghai'
}

// ─── Hardware catalog ──────────────────────────────

resource cisco 'Manufacturer' = {
  name: 'Cisco'
  description: 'Cisco Systems, Inc.'
}

resource juniper 'Manufacturer' = {
  name: 'Juniper'
  description: 'Juniper Networks'
}

resource dell 'Manufacturer' = {
  name: 'Dell'
  description: 'Dell Technologies'
}

// ─── Device Roles ──────────────────────────────────

resource roleRouter 'DeviceRole' = {
  name: 'Router'
  color: 'aa1409'
  description: 'Core and edge routers'
}

resource roleSwitch 'DeviceRole' = {
  name: 'Switch'
  color: '2196f3'
  description: 'Access and distribution switches'
}

resource roleFirewall 'DeviceRole' = {
  name: 'Firewall'
  color: 'ff9800'
  description: 'Network firewalls'
}

resource roleServer 'DeviceRole' = {
  name: 'Server'
  color: '4caf50'
  description: 'Physical servers'
}
