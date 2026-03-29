targetScope = 'local'

@secure()
param netboxToken string
param netboxUrl string

extension netbox with {
  url: netboxUrl
  token: netboxToken
}

// ─── Tunnel Group ─────────────────────────────────

resource tgSiteToSite 'TunnelGroup' = {
  name: 'Site-to-Site VPNs'
  description: 'Site-to-site IPSec tunnels'
}

// ─── IKE Proposals ────────────────────────────────

resource ikeProposal 'IKEProposal' = {
  name: 'IKE-AES256-SHA256-DH14'
  authenticationMethod: 'preshared-keys'
  encryptionAlgorithm: 'aes-256-cbc'
  authenticationAlgorithm: 'hmac-sha256'
  group: '14'
  saLifetime: '86400'
  description: 'AES-256 with SHA-256, DH Group 14'
}

// ─── IKE Policy (references proposal) ─────────────

resource ikePolicy 'IKEPolicy' = {
  name: 'IKE-Policy-Primary'
  version: '2'
  description: 'IKEv2 policy'
}

// ─── IPSec Proposals ──────────────────────────────

resource ipsecProposal 'IPSecProposal' = {
  name: 'ESP-AES256-SHA256'
  encryptionAlgorithm: 'aes-256-cbc'
  authenticationAlgorithm: 'hmac-sha256'
  saLifetimeSeconds: '3600'
  description: 'ESP with AES-256 and SHA-256'
}

// ─── IPSec Policy ─────────────────────────────────

resource ipsecPolicy 'IPSecPolicy' = {
  name: 'IPSec-Policy-Standard'
  pfsGroup: '14'
  description: 'Standard IPSec policy with PFS DH14'
}

// ─── IPSec Profile (references IKE + IPSec policies) ──

resource ipsecProfile 'IPSecProfile' = {
  name: 'Profile-SiteToSite'
  mode: 'esp'
  ikePolicy: ikePolicy.id
  ipsecPolicy: ipsecPolicy.id
  description: 'Site-to-site IPSec profile'
}

// ─── Tunnels (references group + profile) ─────────

resource tunnelStockholmParis 'Tunnel' = {
  name: 'VPN-Stockholm-Paris'
  status: 'active'
  encapsulation: 'ipsec-tunnel'
  group: tgSiteToSite.id
  ipsecProfile: ipsecProfile.id
  description: 'IPSec tunnel Stockholm to Paris'
}

resource tunnelStockholmTokyo 'Tunnel' = {
  name: 'VPN-Stockholm-Tokyo'
  status: 'active'
  encapsulation: 'ipsec-tunnel'
  group: tgSiteToSite.id
  ipsecProfile: ipsecProfile.id
  description: 'IPSec tunnel Stockholm to Tokyo'
}

// ─── L2VPN ────────────────────────────────────────

resource l2vpnDci 'L2VPN' = {
  name: 'DCI-VXLAN'
  type: 'vxlan'
  status: 'active'
  description: 'Data center interconnect via VXLAN'
}
