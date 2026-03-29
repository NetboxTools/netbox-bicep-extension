# NetBox Bicep Extension - Resource Reference

This wiki documents all resource types available in the NetBox Bicep extension. Each resource type maps to a NetBox REST API endpoint and can be declared in Bicep using infrastructure-as-code patterns.

## Configuration

All resources require the NetBox extension to be configured with a URL and API token:

```bicep
extension netbox with {
  url: 'https://netbox.example.com'
  token: 'your-api-token'
}
```

## Resource Types

### DCIM (Data Center Infrastructure Management)

| Resource Type | Description | API Endpoint |
|---|---|---|
| [Site](Site) | A physical location (datacenter, office, etc.) | `/api/dcim/sites/` |
| [Region](Region) | A geographic region (e.g. continent, country, state) | `/api/dcim/regions/` |
| [SiteGroup](SiteGroup) | A logical group of sites (e.g. corporate, branch, edge) | `/api/dcim/site-groups/` |
| [Manufacturer](Manufacturer) | A hardware manufacturer (e.g. Cisco, Juniper, Dell) | `/api/dcim/manufacturers/` |
| [Platform](Platform) | A device platform (OS/firmware, e.g. Cisco IOS, Junos) | `/api/dcim/platforms/` |
| [DeviceRole](DeviceRole) | A functional role for devices (e.g. router, switch, firewall) | `/api/dcim/device-roles/` |
| [DeviceType](DeviceType) | A device hardware model (e.g. Cisco Catalyst 9300) | `/api/dcim/device-types/` |
| [Device](Device) | A physical device (server, switch, router, etc.) | `/api/dcim/devices/` |
| [Interface](Interface) | A physical network interface on a device | `/api/dcim/interfaces/` |
| [Location](Location) | A location within a site (e.g. building, floor, room) | `/api/dcim/locations/` |
| [Rack](Rack) | A physical equipment rack | `/api/dcim/racks/` |
| [RackRole](RackRole) | A functional role for racks (e.g. Compute, Storage) | `/api/dcim/rack-roles/` |
| [RackType](RackType) | A rack hardware model (template for racks) | `/api/dcim/rack-types/` |

### IPAM (IP Address Management)

| Resource Type | Description | API Endpoint |
|---|---|---|
| [Prefix](Prefix) | An IP prefix (subnet) | `/api/ipam/prefixes/` |
| [IPAddress](IPAddress) | An individual IP address | `/api/ipam/ip-addresses/` |
| [IPRange](IPRange) | An arbitrary IP address range | `/api/ipam/ip-ranges/` |
| [VLAN](VLAN) | A VLAN | `/api/ipam/vlans/` |
| [VLANGroup](VLANGroup) | A VLAN group for organizing VLANs | `/api/ipam/vlan-groups/` |
| [VLANTranslationPolicy](VLANTranslationPolicy) | A VLAN translation policy | `/api/ipam/vlan-translation-policies/` |
| [VRF](VRF) | A VRF (Virtual Routing and Forwarding) instance | `/api/ipam/vrfs/` |
| [RouteTarget](RouteTarget) | A BGP route target | `/api/ipam/route-targets/` |
| [RIR](RIR) | A Regional Internet Registry (e.g. ARIN, RIPE, APNIC) | `/api/ipam/rirs/` |
| [Aggregate](Aggregate) | A top-level IP aggregate (allocated by a RIR) | `/api/ipam/aggregates/` |
| [IPAMRole](IPAMRole) | An IPAM role for prefixes and VLANs | `/api/ipam/roles/` |
| [ASN](ASN) | An Autonomous System Number | `/api/ipam/asns/` |
| [ASNRange](ASNRange) | A range of Autonomous System Numbers | `/api/ipam/asn-ranges/` |

### Tenancy

| Resource Type | Description | API Endpoint |
|---|---|---|
| [Tenant](Tenant) | A tenant (customer, department, or business unit) | `/api/tenancy/tenants/` |
| [TenantGroup](TenantGroup) | A group of tenants (e.g. by department) | `/api/tenancy/tenant-groups/` |
| [Contact](Contact) | A contact person | `/api/tenancy/contacts/` |
| [ContactGroup](ContactGroup) | A group of contacts | `/api/tenancy/contact-groups/` |
| [ContactRole](ContactRole) | A functional role for contacts (e.g. NOC, billing) | `/api/tenancy/contact-roles/` |

### Virtualization

| Resource Type | Description | API Endpoint |
|---|---|---|
| [ClusterType](ClusterType) | A cluster technology type (e.g. VMware, Hyper-V) | `/api/virtualization/cluster-types/` |
| [ClusterGroup](ClusterGroup) | A logical group of clusters | `/api/virtualization/cluster-groups/` |
| [Cluster](Cluster) | A virtualization cluster (e.g. vCenter, AKS) | `/api/virtualization/clusters/` |
| [VirtualMachine](VirtualMachine) | A virtual machine | `/api/virtualization/virtual-machines/` |
| [VMInterface](VMInterface) | A network interface on a virtual machine | `/api/virtualization/interfaces/` |
| [VirtualDisk](VirtualDisk) | A virtual disk attached to a virtual machine | `/api/virtualization/virtual-disks/` |

### Circuits

| Resource Type | Description | API Endpoint |
|---|---|---|
| [CircuitType](CircuitType) | A type of circuit (e.g. Internet, MPLS, Dark Fiber) | `/api/circuits/circuit-types/` |
| [Provider](Provider) | A circuit provider (e.g. AT&T, Zayo, Lumen) | `/api/circuits/providers/` |
| [Circuit](Circuit) | A communications circuit (e.g. Internet, MPLS) | `/api/circuits/circuits/` |

### VPN

| Resource Type | Description | API Endpoint |
|---|---|---|
| [TunnelGroup](TunnelGroup) | A logical group of VPN tunnels | `/api/vpn/tunnel-groups/` |
| [Tunnel](Tunnel) | A VPN tunnel | `/api/vpn/tunnels/` |
| [IKEProposal](IKEProposal) | An IKE (Internet Key Exchange) proposal | `/api/vpn/ike-proposals/` |
| [IKEPolicy](IKEPolicy) | An IKE (Internet Key Exchange) policy | `/api/vpn/ike-policies/` |
| [IPSecProposal](IPSecProposal) | An IPSec proposal (transform set) | `/api/vpn/ipsec-proposals/` |
| [IPSecPolicy](IPSecPolicy) | An IPSec policy (grouping of IPSec proposals) | `/api/vpn/ipsec-policies/` |
| [IPSecProfile](IPSecProfile) | An IPSec profile (links IKE policy and IPSec policy) | `/api/vpn/ipsec-profiles/` |
| [L2VPN](L2VPN) | A Layer 2 VPN (e.g. VPLS, VXLAN, EVPN) | `/api/vpn/l2vpns/` |

### Wireless

| Resource Type | Description | API Endpoint |
|---|---|---|
| [WirelessLANGroup](WirelessLANGroup) | A logical group of wireless LANs | `/api/wireless/wireless-lan-groups/` |
| [WirelessLAN](WirelessLAN) | A wireless LAN (Wi-Fi network) | `/api/wireless/wireless-lans/` |
| [WirelessLink](WirelessLink) | A point-to-point wireless link between two interfaces | `/api/wireless/wireless-links/` |

### Extras

| Resource Type | Description | API Endpoint |
|---|---|---|
| [Tag](Tag) | A tag for labeling and filtering objects | `/api/extras/tags/` |

### Users

| Resource Type | Description | API Endpoint |
|---|---|---|
| [User](User) | A NetBox user account | `/api/users/users/` |
| [UserGroup](UserGroup) | A user group for permissions | `/api/users/groups/` |

## Output Properties

All resources return the following read-only output properties after creation:

| Property | Type | Description |
|---|---|---|
| `id` | string | NetBox internal ID |
| `url` | string | API URL for this resource |
| `display` | string | Display name |

These can be referenced in other resource declarations (e.g. `site.id`, `manufacturer.id`).
