targetScope = 'local'

@secure()
param netboxToken string
param netboxUrl string

extension netbox with {
  url: netboxUrl
  token: netboxToken
}

// ─── RIRs ──────────────────────────────────────────────

resource rirRipe 'RIR' = {
  name: 'RIPE NCC'
  description: 'RIPE Network Coordination Centre'
}

resource rirPrivate 'RIR' = {
  name: 'RFC 1918'
  isPrivate: 'true'
  description: 'Private address space'
}

// ─── IPAM Roles ────────────────────────────────────────

resource roleProduction 'IPAMRole' = {
  name: 'Production'
  description: 'Production networks'
}

resource roleDevelopment 'IPAMRole' = {
  name: 'Development'
  description: 'Development/test networks'
}

// ─── VRFs ──────────────────────────────────────────────

resource vrfDefault 'VRF' = {
  name: 'Global'
  description: 'Global routing table'
  enforceUnique: 'true'
}

resource vrfCustomer 'VRF' = {
  name: 'Customer-A'
  rd: '65000:100'
  description: 'Customer A VRF'
}

// ─── Route Targets ─────────────────────────────────────

resource rtImport 'RouteTarget' = {
  name: '65000:100'
  description: 'Import target for Customer A'
}

resource rtExport 'RouteTarget' = {
  name: '65000:200'
  description: 'Export target for Customer A'
}

// ─── VLAN Groups ───────────────────────────────────────

resource vlanGroupDc 'VLANGroup' = {
  name: 'Datacenter VLANs'
  description: 'VLAN pool for datacenter'
}

// ─── IP Ranges ─────────────────────────────────────────

resource dhcpRange 'IPRange' = {
  startAddress: '10.0.10.100/24'
  endAddress: '10.0.10.200/24'
  status: 'active'
  description: 'DHCP pool for management network'
}

// ─── ASNs ──────────────────────────────────────────────

resource asnPrimary 'ASN' = {
  asn: 65000
  rir: rirRipe.id
  description: 'Primary private ASN'
}

resource asnSecondary 'ASN' = {
  asn: 65001
  rir: rirRipe.id
  description: 'Secondary private ASN'
}

// ─── VLAN Translation Policy ───────────────────────────

resource translationPolicy 'VLANTranslationPolicy' = {
  name: 'DC-Interconnect'
  description: 'VLAN translation between datacenters'
}
