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
  description: 'Microsoft Hyper-V'
}

resource ctVmware 'ClusterType' = {
  name: 'VMware vSphere'
  description: 'VMware vSphere / ESXi'
}

resource ctAzure 'ClusterType' = {
  name: 'Azure'
  description: 'Microsoft Azure Cloud'
}

// ─── Cluster Groups ────────────────────────────────────

resource cgProduction 'ClusterGroup' = {
  name: 'Production'
  description: 'Production clusters'
}

resource cgDevelopment 'ClusterGroup' = {
  name: 'Development'
  description: 'Development and test clusters'
}

// ─── Clusters ──────────────────────────────────────────

resource clAzureWestEurope 'Cluster' = {
  name: 'Azure West Europe'
  type: ctAzure.id
  status: 'active'
  description: 'Azure West Europe region'
}

// ─── Virtual Machines ──────────────────────────────────

// Note: VMs require a site or cluster assignment in NetBox v4.5+

resource vmWeb01 'VirtualMachine' = {
  name: 'web-server-01'
  status: 'active'
  cluster: clAzureWestEurope.id
  vcpus: '4'
  memory: '8192'
  disk: '153600'  // 50 GB system + 100 GB data
  description: 'Web server - production'
}

resource vmDb01 'VirtualMachine' = {
  name: 'db-server-01'
  status: 'active'
  cluster: clAzureWestEurope.id
  vcpus: '8'
  memory: '32768'
  disk: '563200'  // 50 GB system + 500 GB data
  description: 'Database server - production'
}

resource vmDev01 'VirtualMachine' = {
  name: 'dev-server-01'
  status: 'active'
  cluster: clAzureWestEurope.id
  vcpus: '2'
  memory: '4096'
  disk: '51200'
  description: 'Development server'
}

// ─── VM Interfaces ────────────────────────────────────

resource ifWeb01Eth0 'VMInterface' = {
  name: 'eth0'
  virtualMachine: vmWeb01.id
  enabled: 'true'
  mtu: '1500'
  description: 'Primary network interface'
}

resource ifDb01Eth0 'VMInterface' = {
  name: 'eth0'
  virtualMachine: vmDb01.id
  enabled: 'true'
  mtu: '9000'
  description: 'Primary network interface (jumbo frames)'
}

resource ifDev01Eth0 'VMInterface' = {
  name: 'eth0'
  virtualMachine: vmDev01.id
  enabled: 'true'
  description: 'Primary network interface'
}

// ─── Virtual Disks ────────────────────────────────────

resource diskWeb01System 'VirtualDisk' = {
  name: 'web01-system'
  virtualMachine: vmWeb01.id
  size: 51200
  description: 'OS disk (50 GB)'
}

resource diskWeb01Data 'VirtualDisk' = {
  name: 'web01-data'
  virtualMachine: vmWeb01.id
  size: 102400
  description: 'Data disk (100 GB)'
}

resource diskDb01System 'VirtualDisk' = {
  name: 'db01-system'
  virtualMachine: vmDb01.id
  size: 51200
  description: 'OS disk (50 GB)'
}

resource diskDb01Data 'VirtualDisk' = {
  name: 'db01-data'
  virtualMachine: vmDb01.id
  size: 512000
  description: 'Database disk (500 GB)'
}

// ─── IP Addresses ─────────────────────────────────────

resource ipWeb01 'IPAddress' = {
  address: '10.10.1.10/32'
  status: 'active'
  dnsName: 'web-server-01.contoso.com'
  assignedObjectType: 'virtualization.vminterface'
  assignedObjectId: ifWeb01Eth0.id
  description: 'web-server-01 primary IP'
}

resource ipDb01 'IPAddress' = {
  address: '10.10.1.20/32'
  status: 'active'
  dnsName: 'db-server-01.contoso.com'
  assignedObjectType: 'virtualization.vminterface'
  assignedObjectId: ifDb01Eth0.id
  description: 'db-server-01 primary IP'
}

resource ipDev01 'IPAddress' = {
  address: '10.10.2.10/32'
  status: 'active'
  dnsName: 'dev-server-01.contoso.com'
  assignedObjectType: 'virtualization.vminterface'
  assignedObjectId: ifDev01Eth0.id
  description: 'dev-server-01 primary IP'
}
