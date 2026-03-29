targetScope = 'local'

@secure()
param netboxToken string
param netboxUrl string

extension netbox with {
  url: netboxUrl
  token: netboxToken
}

// ─── Circuit Types ────────────────────────────────

resource ctDarkFiber 'CircuitType' = {
  name: 'Dark Fiber'
  color: '111111'
  description: 'Unlit fiber optic circuit'
}

resource ctMPLS 'CircuitType' = {
  name: 'MPLS'
  color: '2196f3'
  description: 'MPLS L3VPN circuit'
}

resource ctInternet 'CircuitType' = {
  name: 'Internet Transit'
  color: '4caf50'
  description: 'Internet transit circuit'
}

// ─── Providers ────────────────────────────────────

resource provTelia 'Provider' = {
  name: 'Telia Carrier'
  description: 'Telia Carrier — Nordic backbone provider'
}

resource provCogent 'Provider' = {
  name: 'Cogent Communications'
  description: 'Cogent — Tier 1 global transit provider'
}

// ─── Circuits (referencing type + provider via .id) ──

resource circStockholmParis 'Circuit' = {
  cid: 'TEL-STH-PAR-001'
  provider: provTelia.id
  type: ctDarkFiber.id
  status: 'active'
  commitRate: '100000'
  description: 'Stockholm-Paris dark fiber 100G'
}

resource circStockholmTokyo 'Circuit' = {
  cid: 'TEL-STH-TYO-001'
  provider: provTelia.id
  type: ctMPLS.id
  status: 'active'
  commitRate: '10000'
  description: 'Stockholm-Tokyo MPLS 10G'
}

resource circInternetPrimary 'Circuit' = {
  cid: 'COG-IX-001'
  provider: provCogent.id
  type: ctInternet.id
  status: 'active'
  commitRate: '10000'
  description: 'Primary internet transit 10G'
}
