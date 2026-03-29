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
  slug: 'ripe-ncc'
  description: 'RIPE Network Coordination Centre'
}

resource rirPrivate 'RIR' = {
  name: 'RFC 1918'
  slug: 'rfc-1918'
  isPrivate: 'true'
  description: 'Private address space'
}

// ─── IPAM Roles ────────────────────────────────────────

resource roleProduction 'IPAMRole' = {
  name: 'Production'
  slug: 'production'
  description: 'Production networks'
}

resource roleDevelopment 'IPAMRole' = {
  name: 'Development'
  slug: 'development'
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
  slug: 'datacenter-vlans'
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
  rir: 1  // Update with actual RIR ID after deploying RIRs above
  description: 'Primary private ASN'
}

resource asnSecondary 'ASN' = {
  asn: 65001
  rir: 1  // Update with actual RIR ID after deploying RIRs above
  description: 'Secondary private ASN'
}

// ─── VLAN Translation Policy ───────────────────────────

resource translationPolicy 'VLANTranslationPolicy' = {
  name: 'DC-Interconnect'
  description: 'VLAN translation between datacenters'
}
