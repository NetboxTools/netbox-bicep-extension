targetScope = 'local'

@secure()
param netboxToken string

param netboxUrl string

extension netbox with {
  url: netboxUrl
  token: netboxToken
}

// ─── VLANs ─────────────────────────────────────────

resource vlanMgmt 'VLAN' = {
  vid: 10
  name: 'Management'
  status: 'active'
  description: 'Out-of-band management'
}

resource vlanServers 'VLAN' = {
  vid: 100
  name: 'Servers'
  status: 'active'
  description: 'Server production traffic'
}

resource vlanStorage 'VLAN' = {
  vid: 200
  name: 'Storage'
  status: 'active'
  description: 'iSCSI / NFS storage traffic'
}

resource vlanGuest 'VLAN' = {
  vid: 999
  name: 'Guest'
  status: 'active'
  description: 'Guest WiFi network'
}

// ─── Prefixes ──────────────────────────────────────

resource prefixMgmt 'Prefix' = {
  prefix: '10.0.10.0/24'
  status: 'active'
  description: 'Management network'
}

resource prefixServers 'Prefix' = {
  prefix: '10.0.100.0/24'
  status: 'active'
  description: 'Server network'
}

resource prefixStorage 'Prefix' = {
  prefix: '10.0.200.0/24'
  status: 'active'
  description: 'Storage network'
}

resource prefixGuest 'Prefix' = {
  prefix: '192.168.1.0/24'
  status: 'active'
  description: 'Guest WiFi'
}

// ─── Key IP Addresses ──────────────────────────────

resource gwMgmt 'IPAddress' = {
  address: '10.0.10.1/24'
  status: 'active'
  description: 'Management gateway'
  dnsName: 'gw-mgmt.contoso.local'
}

resource gwServers 'IPAddress' = {
  address: '10.0.100.1/24'
  status: 'active'
  description: 'Server gateway'
  dnsName: 'gw-servers.contoso.local'
}

resource gwStorage 'IPAddress' = {
  address: '10.0.200.1/24'
  status: 'active'
  description: 'Storage gateway'
  dnsName: 'gw-storage.contoso.local'
}
