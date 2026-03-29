targetScope = 'local'

@secure()
param netboxToken string
param netboxUrl string

extension netbox with {
  url: netboxUrl
  token: netboxToken
}

// ─── Cluster Types ─────────────────────────────────────

resource ctHyperV 'ClusterType' = {
  name: 'Hyper-V'
  slug: 'hyper-v'
  description: 'Microsoft Hyper-V'
}

resource ctVmware 'ClusterType' = {
  name: 'VMware vSphere'
  slug: 'vmware-vsphere'
  description: 'VMware vSphere / ESXi'
}

resource ctAzure 'ClusterType' = {
  name: 'Azure'
  slug: 'azure'
  description: 'Microsoft Azure Cloud'
}

// ─── Cluster Groups ────────────────────────────────────

resource cgProduction 'ClusterGroup' = {
  name: 'Production'
  slug: 'production'
  description: 'Production clusters'
}

resource cgDevelopment 'ClusterGroup' = {
  name: 'Development'
  slug: 'development'
  description: 'Development and test clusters'
}

// ─── Clusters ──────────────────────────────────────────

resource clAzureWestEurope 'Cluster' = {
  name: 'Azure West Europe'
  type: 1  // Update with ClusterType ID after first deploy
  status: 'active'
  description: 'Azure West Europe region'
}

// ─── Virtual Machines ──────────────────────────────────

// Note: VMs require a site or cluster assignment in NetBox v4.5+
// Update cluster ID after deploying clusters above

resource vmWeb01 'VirtualMachine' = {
  name: 'web-server-01'
  status: 'active'
  cluster: '1'  // Update with actual Cluster ID
  vcpus: '4'
  memory: '8192'
  disk: '102400'
  description: 'Web server - production'
}

resource vmDb01 'VirtualMachine' = {
  name: 'db-server-01'
  status: 'active'
  cluster: '1'  // Update with actual Cluster ID
  vcpus: '8'
  memory: '32768'
  disk: '512000'
  description: 'Database server - production'
}

resource vmDev01 'VirtualMachine' = {
  name: 'dev-server-01'
  status: 'active'
  cluster: '1'  // Update with actual Cluster ID
  vcpus: '2'
  memory: '4096'
  disk: '51200'
  description: 'Development server'
}
