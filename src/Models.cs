using Azure.Bicep.Types.Concrete;
using Bicep.Local.Extension.Types.Attributes;

namespace Bicep.Extension.Netbox;

// ──────────────────────────────────────────────
// Shared output interface — implemented by all resource models
// ──────────────────────────────────────────────

/// <summary>
/// Interface for output properties returned by the NetBox API after create/update.
/// Allows Bicep to reference resource IDs: site.id, manufacturer.id, etc.
/// </summary>
public interface INetboxResource
{
    string? Id { get; set; }
    string? Url { get; set; }
    string? Display { get; set; }
}

/// <summary>
/// Extension-level configuration passed via: extension netbox with { url: '...', token: '...' }
/// </summary>
public class Configuration
{
    [TypeProperty("The base URL of the NetBox instance (e.g. https://netbox.example.com).", ObjectTypePropertyFlags.Required)]

    public required string Url { get; set; }

    [TypeProperty("The NetBox API token for authentication.", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.WriteOnly)]
    public required string Token { get; set; }
}

// ──────────────────────────────────────────────
// Shared identifier bases
// ──────────────────────────────────────────────

/// <summary>
/// Identifier for resources that use name + auto-generated slug.
/// Slug is computed from name if not provided (e.g. "Stockholm DC1" → "stockholm-dc1").
/// Used by most slug-based resources: Site, Tenant, Manufacturer, DeviceRole, etc.
/// </summary>
public class NameSlugIdentifiers : INetboxResource
{
    [TypeProperty("Unique name.", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required string Name { get; set; }

    [TypeProperty("URL-friendly identifier (auto-generated from name if omitted, e.g. 'my-datacenter').", ObjectTypePropertyFlags.None)]
    public string? Slug { get; set; }

    [TypeProperty("NetBox internal ID (read-only, returned after create/update).", ObjectTypePropertyFlags.ReadOnly)]
    public string? Id { get; set; }

    [TypeProperty("API URL for this resource.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Url { get; set; }

    [TypeProperty("Display name.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Display { get; set; }
}

/// <summary>
/// Identifier for resources that use slug as their unique key but don't have a standard 'name' field.
/// Used by: DeviceType, RackType (which use 'model' instead of 'name').
/// </summary>
public class SlugIdentifiers : INetboxResource
{
    [TypeProperty("URL-friendly unique identifier (e.g. 'catalyst-9300').", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required string Slug { get; set; }

    [TypeProperty("NetBox internal ID (read-only, returned after create/update).", ObjectTypePropertyFlags.ReadOnly)]
    public string? Id { get; set; }

    [TypeProperty("API URL for this resource.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Url { get; set; }

    [TypeProperty("Display name.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Display { get; set; }
}

// ──────────────────────────────────────────────
// DCIM: Sites, Manufacturers, DeviceRoles, DeviceTypes, Devices
// ──────────────────────────────────────────────

/// <summary>
/// A physical location (datacenter, office, etc.).
/// API: /api/dcim/sites/
/// </summary>
[ResourceType("Site")]
public class Site : NameSlugIdentifiers
{
    [TypeProperty("Operational status: planned, staging, active, decommissioning, retired.", ObjectTypePropertyFlags.None)]
    public string? Status { get; set; }

    [TypeProperty("The region ID.", ObjectTypePropertyFlags.None)]
    public string? Region { get; set; }

    [TypeProperty("Local facility ID or description.", ObjectTypePropertyFlags.None)]
    public string? Facility { get; set; }

    [TypeProperty("A brief description of the site.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Physical address of the site.", ObjectTypePropertyFlags.None)]
    public string? PhysicalAddress { get; set; }

    [TypeProperty("Shipping address (if different from physical).", ObjectTypePropertyFlags.None)]
    public string? ShippingAddress { get; set; }

    [TypeProperty("IANA time zone (e.g. 'America/New_York').", ObjectTypePropertyFlags.None)]
    public string? TimeZone { get; set; }

    [TypeProperty("GPS latitude (-90 to 90) as a string (e.g. '59.3293').", ObjectTypePropertyFlags.None)]
    public string? Latitude { get; set; }

    [TypeProperty("GPS longitude (-180 to 180) as a string (e.g. '18.0686').", ObjectTypePropertyFlags.None)]
    public string? Longitude { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// A hardware manufacturer (e.g. Cisco, Juniper, Dell).
/// API: /api/dcim/manufacturers/
/// </summary>
[ResourceType("Manufacturer")]
public class Manufacturer : NameSlugIdentifiers
{
    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// A functional role for devices (e.g. router, switch, firewall).
/// API: /api/dcim/device-roles/
/// </summary>
[ResourceType("DeviceRole")]
public class DeviceRole : NameSlugIdentifiers
{
    [TypeProperty("Color in hex (e.g. 'aa1409').", ObjectTypePropertyFlags.None)]
    public string? Color { get; set; }

    [TypeProperty("Whether this role can be assigned to virtual machines (true/false).", ObjectTypePropertyFlags.None)]
    public string? VmRole { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// A device hardware model (e.g. Cisco Catalyst 9300, Dell PowerEdge R750).
/// API: /api/dcim/device-types/
/// </summary>
[ResourceType("DeviceType")]
public class DeviceType : SlugIdentifiers
{
    [TypeProperty("The manufacturer ID.", ObjectTypePropertyFlags.Required)]
    public required string Manufacturer { get; set; }

    [TypeProperty("Model name.", ObjectTypePropertyFlags.Required)]
    public required string Model { get; set; }

    [TypeProperty("Part number.", ObjectTypePropertyFlags.None)]
    public string? PartNumber { get; set; }

    [TypeProperty("Height in rack units.", ObjectTypePropertyFlags.None)]
    public string? UHeight { get; set; }

    [TypeProperty("Whether the device takes up the full depth of a rack (true/false).", ObjectTypePropertyFlags.None)]
    public string? IsFullDepth { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// Identifier for a Device. Uses name as the unique key (unique per site).
/// </summary>
public class DeviceIdentifiers : INetboxResource
{
    [TypeProperty("Device name (unique per site).", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required string Name { get; set; }

    [TypeProperty("NetBox internal ID (read-only, returned after create/update).", ObjectTypePropertyFlags.ReadOnly)]
    public string? Id { get; set; }

    [TypeProperty("API URL for this resource.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Url { get; set; }

    [TypeProperty("Display name.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Display { get; set; }
}

/// <summary>
/// A physical device (server, switch, router, etc.).
/// API: /api/dcim/devices/
/// </summary>
[ResourceType("Device")]
public class Device : DeviceIdentifiers
{
    [TypeProperty("The device type ID (hardware model).", ObjectTypePropertyFlags.Required)]
    public required string DeviceType { get; set; }

    [TypeProperty("The device role ID.", ObjectTypePropertyFlags.Required)]
    public required string Role { get; set; }

    [TypeProperty("The site ID where this device is located.", ObjectTypePropertyFlags.Required)]
    public required string Site { get; set; }

    [TypeProperty("Operational status: offline, active, planned, staged, failed, inventory, decommissioning.", ObjectTypePropertyFlags.None)]
    public string? Status { get; set; }

    [TypeProperty("The rack ID.", ObjectTypePropertyFlags.None)]
    public string? Rack { get; set; }

    [TypeProperty("Position in rack (starting from bottom).", ObjectTypePropertyFlags.None)]
    public string? Position { get; set; }

    [TypeProperty("Rack face: front, rear.", ObjectTypePropertyFlags.None)]
    public string? Face { get; set; }

    [TypeProperty("The tenant ID.", ObjectTypePropertyFlags.None)]
    public string? Tenant { get; set; }

    [TypeProperty("The platform ID (OS/firmware).", ObjectTypePropertyFlags.None)]
    public string? Platform { get; set; }

    [TypeProperty("Serial number.", ObjectTypePropertyFlags.None)]
    public string? Serial { get; set; }

    [TypeProperty("Unique asset tag.", ObjectTypePropertyFlags.None)]
    public string? AssetTag { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

// ──────────────────────────────────────────────
// Tenancy
// ──────────────────────────────────────────────

/// <summary>
/// A tenant (customer, department, or business unit).
/// API: /api/tenancy/tenants/
/// </summary>
[ResourceType("Tenant")]
public class Tenant : NameSlugIdentifiers
{
    [TypeProperty("The tenant group ID.", ObjectTypePropertyFlags.None)]
    public string? Group { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

// ──────────────────────────────────────────────
// Shared base: resources that use name as their unique identifier
// ──────────────────────────────────────────────

/// <summary>
/// Shared identifier for resources that use name as their unique key.
/// Used by: VRF, RouteTarget, VLANTranslationPolicy.
/// </summary>
public class NameIdentifiers : INetboxResource
{
    [TypeProperty("Unique name.", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required string Name { get; set; }

    [TypeProperty("NetBox internal ID (read-only, returned after create/update).", ObjectTypePropertyFlags.ReadOnly)]
    public string? Id { get; set; }

    [TypeProperty("API URL for this resource.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Url { get; set; }

    [TypeProperty("Display name.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Display { get; set; }
}

// ──────────────────────────────────────────────
// IPAM: Prefixes, IP Addresses, VLANs, VRFs, and more
// ──────────────────────────────────────────────

/// <summary>
/// Identifier for a Prefix. Uses the CIDR notation as the unique key.
/// </summary>
public class PrefixIdentifiers : INetboxResource
{
    [TypeProperty("IP prefix in CIDR notation (e.g. '10.0.0.0/24').", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required string Prefix { get; set; }

    [TypeProperty("NetBox internal ID (read-only, returned after create/update).", ObjectTypePropertyFlags.ReadOnly)]
    public string? Id { get; set; }

    [TypeProperty("API URL for this resource.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Url { get; set; }

    [TypeProperty("Display name.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Display { get; set; }
}

/// <summary>
/// An IP prefix (subnet).
/// API: /api/ipam/prefixes/
/// </summary>
[ResourceType("Prefix")]
public class Prefix : PrefixIdentifiers
{
    [TypeProperty("Operational status: container, active, reserved, deprecated.", ObjectTypePropertyFlags.None)]
    public string? Status { get; set; }

    [TypeProperty("The VRF ID.", ObjectTypePropertyFlags.None)]
    public string? Vrf { get; set; }

    [TypeProperty("The tenant ID.", ObjectTypePropertyFlags.None)]
    public string? Tenant { get; set; }

    [TypeProperty("The VLAN ID.", ObjectTypePropertyFlags.None)]
    public string? Vlan { get; set; }

    [TypeProperty("The IPAM role ID.", ObjectTypePropertyFlags.None)]
    public string? Role { get; set; }

    [TypeProperty("Treat this prefix as a pool of available addresses (true/false).", ObjectTypePropertyFlags.None)]
    public string? IsPool { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// Identifier for an IP Address. Uses CIDR notation as the unique key.
/// </summary>
public class IPAddressIdentifiers : INetboxResource
{
    [TypeProperty("IP address in CIDR notation (e.g. '10.0.0.1/24').", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required string Address { get; set; }

    [TypeProperty("NetBox internal ID (read-only, returned after create/update).", ObjectTypePropertyFlags.ReadOnly)]
    public string? Id { get; set; }

    [TypeProperty("API URL for this resource.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Url { get; set; }

    [TypeProperty("Display name.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Display { get; set; }
}

/// <summary>
/// An individual IP address.
/// API: /api/ipam/ip-addresses/
/// </summary>
[ResourceType("IPAddress")]
public class IPAddress : IPAddressIdentifiers
{
    [TypeProperty("Operational status: active, reserved, deprecated, dhcp, slaac.", ObjectTypePropertyFlags.None)]
    public string? Status { get; set; }

    [TypeProperty("Functional role: loopback, secondary, anycast, vip, vrrp, hsrp, glbp, carp.", ObjectTypePropertyFlags.None)]
    public string? Role { get; set; }

    [TypeProperty("The VRF ID.", ObjectTypePropertyFlags.None)]
    public string? Vrf { get; set; }

    [TypeProperty("The tenant ID.", ObjectTypePropertyFlags.None)]
    public string? Tenant { get; set; }

    [TypeProperty("DNS hostname.", ObjectTypePropertyFlags.None)]
    public string? DnsName { get; set; }

    [TypeProperty("Assigned object type (e.g. 'virtualization.vminterface' or 'dcim.interface').", ObjectTypePropertyFlags.None)]
    public string? AssignedObjectType { get; set; }

    [TypeProperty("Assigned object ID (the interface ID to attach this IP to).", ObjectTypePropertyFlags.None)]
    public string? AssignedObjectId { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// Identifier for a VLAN. Uses vid (VLAN ID number) as the unique key.
/// </summary>
public class VLANIdentifiers : INetboxResource
{
    [TypeProperty("VLAN ID number (1-4094).", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required int Vid { get; set; }

    [TypeProperty("NetBox internal ID (read-only, returned after create/update).", ObjectTypePropertyFlags.ReadOnly)]
    public string? Id { get; set; }

    [TypeProperty("API URL for this resource.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Url { get; set; }

    [TypeProperty("Display name.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Display { get; set; }
}

/// <summary>
/// A VLAN.
/// API: /api/ipam/vlans/
/// </summary>
[ResourceType("VLAN")]
public class VLAN : VLANIdentifiers
{
    [TypeProperty("VLAN name.", ObjectTypePropertyFlags.Required)]
    public required string Name { get; set; }

    [TypeProperty("Operational status: active, reserved, deprecated.", ObjectTypePropertyFlags.None)]
    public string? Status { get; set; }

    [TypeProperty("The VLAN group ID.", ObjectTypePropertyFlags.None)]
    public string? Group { get; set; }

    [TypeProperty("The site ID.", ObjectTypePropertyFlags.None)]
    public string? Site { get; set; }

    [TypeProperty("The tenant ID.", ObjectTypePropertyFlags.None)]
    public string? Tenant { get; set; }

    [TypeProperty("The IPAM role ID.", ObjectTypePropertyFlags.None)]
    public string? Role { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// A VRF (Virtual Routing and Forwarding) instance.
/// API: /api/ipam/vrfs/
/// </summary>
[ResourceType("VRF")]
public class VRF : NameIdentifiers
{
    [TypeProperty("Route distinguisher (e.g. '65000:1').", ObjectTypePropertyFlags.None)]
    public string? Rd { get; set; }

    [TypeProperty("Enforce unique IP space within this VRF (true/false).", ObjectTypePropertyFlags.None)]
    public string? EnforceUnique { get; set; }

    [TypeProperty("The tenant ID.", ObjectTypePropertyFlags.None)]
    public string? Tenant { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// A BGP route target.
/// API: /api/ipam/route-targets/
/// </summary>
[ResourceType("RouteTarget")]
public class RouteTarget : NameIdentifiers
{
    [TypeProperty("The tenant ID.", ObjectTypePropertyFlags.None)]
    public string? Tenant { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// A Regional Internet Registry (e.g. ARIN, RIPE, APNIC).
/// API: /api/ipam/rirs/
/// </summary>
[ResourceType("RIR")]
public class RIR : NameSlugIdentifiers
{
    [TypeProperty("Whether this is a private registry (true/false).", ObjectTypePropertyFlags.None)]
    public string? IsPrivate { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// Identifier for an Aggregate. Uses CIDR prefix as the unique key.
/// </summary>
public class AggregateIdentifiers : INetboxResource
{
    [TypeProperty("Aggregate prefix in CIDR notation (e.g. '10.0.0.0/8').", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required string Prefix { get; set; }

    [TypeProperty("NetBox internal ID (read-only, returned after create/update).", ObjectTypePropertyFlags.ReadOnly)]
    public string? Id { get; set; }

    [TypeProperty("API URL for this resource.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Url { get; set; }

    [TypeProperty("Display name.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Display { get; set; }
}

/// <summary>
/// A top-level IP aggregate (allocated by a RIR).
/// API: /api/ipam/aggregates/
/// </summary>
[ResourceType("Aggregate")]
public class Aggregate : AggregateIdentifiers
{
    [TypeProperty("The RIR ID.", ObjectTypePropertyFlags.Required)]
    public required string Rir { get; set; }

    [TypeProperty("The tenant ID.", ObjectTypePropertyFlags.None)]
    public string? Tenant { get; set; }

    [TypeProperty("Date added (yyyy-MM-dd).", ObjectTypePropertyFlags.None)]
    public string? DateAdded { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// An IPAM role for prefixes and VLANs (e.g. Production, Development).
/// API: /api/ipam/roles/
/// </summary>
[ResourceType("IPAMRole")]
public class IPAMRole : NameSlugIdentifiers
{
    [TypeProperty("Sort weight for ordering.", ObjectTypePropertyFlags.None)]
    public string? Weight { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// Identifier for an IP Range.
/// </summary>
public class IPRangeIdentifiers : INetboxResource
{
    [TypeProperty("Start IP address in CIDR notation (e.g. '10.0.0.100/24').", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required string StartAddress { get; set; }

    [TypeProperty("NetBox internal ID (read-only, returned after create/update).", ObjectTypePropertyFlags.ReadOnly)]
    public string? Id { get; set; }

    [TypeProperty("API URL for this resource.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Url { get; set; }

    [TypeProperty("Display name.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Display { get; set; }
}

/// <summary>
/// An arbitrary IP address range.
/// API: /api/ipam/ip-ranges/
/// </summary>
[ResourceType("IPRange")]
public class IPRange : IPRangeIdentifiers
{
    [TypeProperty("End IP address in CIDR notation (e.g. '10.0.0.200/24').", ObjectTypePropertyFlags.Required)]
    public required string EndAddress { get; set; }

    [TypeProperty("Operational status: active, reserved, deprecated.", ObjectTypePropertyFlags.None)]
    public string? Status { get; set; }

    [TypeProperty("The VRF ID.", ObjectTypePropertyFlags.None)]
    public string? Vrf { get; set; }

    [TypeProperty("The tenant ID.", ObjectTypePropertyFlags.None)]
    public string? Tenant { get; set; }

    [TypeProperty("The IPAM role ID.", ObjectTypePropertyFlags.None)]
    public string? Role { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// Identifier for an ASN.
/// </summary>
public class ASNIdentifiers : INetboxResource
{
    [TypeProperty("Autonomous System Number (e.g. 65000).", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required int Asn { get; set; }

    [TypeProperty("NetBox internal ID (read-only, returned after create/update).", ObjectTypePropertyFlags.ReadOnly)]
    public string? Id { get; set; }

    [TypeProperty("API URL for this resource.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Url { get; set; }

    [TypeProperty("Display name.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Display { get; set; }
}

/// <summary>
/// An Autonomous System Number.
/// API: /api/ipam/asns/
/// </summary>
[ResourceType("ASN")]
public class ASN : ASNIdentifiers
{
    [TypeProperty("The RIR ID (required in NetBox v4.5+).", ObjectTypePropertyFlags.Required)]
    public required string Rir { get; set; }

    [TypeProperty("The tenant ID.", ObjectTypePropertyFlags.None)]
    public string? Tenant { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// A range of Autonomous System Numbers.
/// API: /api/ipam/asn-ranges/
/// </summary>
[ResourceType("ASNRange")]
public class ASNRange : NameSlugIdentifiers
{
    [TypeProperty("The RIR ID.", ObjectTypePropertyFlags.Required)]
    public required string Rir { get; set; }

    [TypeProperty("Starting ASN.", ObjectTypePropertyFlags.Required)]
    public required int Start { get; set; }

    [TypeProperty("Ending ASN.", ObjectTypePropertyFlags.Required)]
    public required int End { get; set; }

    [TypeProperty("The tenant ID.", ObjectTypePropertyFlags.None)]
    public string? Tenant { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// A VLAN group for organizing VLANs.
/// API: /api/ipam/vlan-groups/
/// </summary>
[ResourceType("VLANGroup")]
public class VLANGroup : NameSlugIdentifiers
{
    [TypeProperty("The tenant ID.", ObjectTypePropertyFlags.None)]
    public string? Tenant { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// A VLAN translation policy.
/// API: /api/ipam/vlan-translation-policies/
/// </summary>
[ResourceType("VLANTranslationPolicy")]
public class VLANTranslationPolicy : NameIdentifiers
{
    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

// ──────────────────────────────────────────────
// Virtualization: ClusterTypes, ClusterGroups, Clusters, VMs, Interfaces, Disks
// ──────────────────────────────────────────────

/// <summary>
/// A cluster technology type (e.g. VMware, Hyper-V, Proxmox).
/// API: /api/virtualization/cluster-types/
/// </summary>
[ResourceType("ClusterType")]
public class ClusterType : NameSlugIdentifiers
{
    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// A logical group of clusters.
/// API: /api/virtualization/cluster-groups/
/// </summary>
[ResourceType("ClusterGroup")]
public class ClusterGroup : NameSlugIdentifiers
{
    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// A virtualization cluster (e.g. a vCenter cluster, AKS cluster).
/// API: /api/virtualization/clusters/
/// </summary>
[ResourceType("Cluster")]
public class Cluster : NameIdentifiers
{
    [TypeProperty("The cluster type ID.", ObjectTypePropertyFlags.Required)]
    public required string Type { get; set; }

    [TypeProperty("Operational status: planned, staging, active, decommissioning, offline.", ObjectTypePropertyFlags.None)]
    public string? Status { get; set; }

    [TypeProperty("The cluster group ID.", ObjectTypePropertyFlags.None)]
    public string? Group { get; set; }

    [TypeProperty("The tenant ID.", ObjectTypePropertyFlags.None)]
    public string? Tenant { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// A virtual machine.
/// API: /api/virtualization/virtual-machines/
/// </summary>
[ResourceType("VirtualMachine")]
public class VirtualMachine : NameIdentifiers
{
    [TypeProperty("Operational status: offline, active, planned, staged, failed, decommissioning, paused.", ObjectTypePropertyFlags.None)]
    public string? Status { get; set; }

    [TypeProperty("The site ID.", ObjectTypePropertyFlags.None)]
    public string? Site { get; set; }

    [TypeProperty("The cluster ID.", ObjectTypePropertyFlags.None)]
    public string? Cluster { get; set; }

    [TypeProperty("The device ID (physical host).", ObjectTypePropertyFlags.None)]
    public string? Device { get; set; }

    [TypeProperty("The device role ID.", ObjectTypePropertyFlags.None)]
    public string? Role { get; set; }

    [TypeProperty("The tenant ID.", ObjectTypePropertyFlags.None)]
    public string? Tenant { get; set; }

    [TypeProperty("The platform ID (OS/firmware).", ObjectTypePropertyFlags.None)]
    public string? Platform { get; set; }

    [TypeProperty("Serial number.", ObjectTypePropertyFlags.None)]
    public string? Serial { get; set; }

    [TypeProperty("Number of virtual CPUs (e.g. '4' or '2.5').", ObjectTypePropertyFlags.None)]
    public string? Vcpus { get; set; }

    [TypeProperty("Memory in MB (e.g. '4096').", ObjectTypePropertyFlags.None)]
    public string? Memory { get; set; }

    [TypeProperty("Disk size in MB (e.g. '102400').", ObjectTypePropertyFlags.None)]
    public string? Disk { get; set; }

    [TypeProperty("Primary IPv4 address ID.", ObjectTypePropertyFlags.None)]
    public string? PrimaryIp4 { get; set; }

    [TypeProperty("Primary IPv6 address ID.", ObjectTypePropertyFlags.None)]
    public string? PrimaryIp6 { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// A network interface on a virtual machine.
/// API: /api/virtualization/interfaces/
/// </summary>
[ResourceType("VMInterface")]
public class VMInterface : NameIdentifiers
{
    [TypeProperty("The virtual machine ID.", ObjectTypePropertyFlags.Required)]
    public required string VirtualMachine { get; set; }

    [TypeProperty("Whether the interface is enabled (true/false).", ObjectTypePropertyFlags.None)]
    public string? Enabled { get; set; }

    [TypeProperty("MTU (1-65536).", ObjectTypePropertyFlags.None)]
    public string? Mtu { get; set; }

    [TypeProperty("802.1Q mode: access, tagged, tagged-all, q-in-q.", ObjectTypePropertyFlags.None)]
    public string? Mode { get; set; }

    [TypeProperty("The parent interface ID.", ObjectTypePropertyFlags.None)]
    public string? Parent { get; set; }

    [TypeProperty("The bridge interface ID.", ObjectTypePropertyFlags.None)]
    public string? Bridge { get; set; }

    [TypeProperty("The untagged VLAN ID.", ObjectTypePropertyFlags.None)]
    public string? UntaggedVlan { get; set; }

    [TypeProperty("The VRF ID.", ObjectTypePropertyFlags.None)]
    public string? Vrf { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }
}

/// <summary>
/// A virtual disk attached to a virtual machine.
/// API: /api/virtualization/virtual-disks/
/// </summary>
[ResourceType("VirtualDisk")]
public class VirtualDisk : NameIdentifiers
{
    [TypeProperty("The virtual machine ID.", ObjectTypePropertyFlags.Required)]
    public required string VirtualMachine { get; set; }

    [TypeProperty("Disk size in MB.", ObjectTypePropertyFlags.Required)]
    public required int Size { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }
}

// ──────────────────────────────────────────────
// DCIM: Racks, RackRoles, Locations
// ──────────────────────────────────────────────

/// <summary>
/// A functional role for racks (e.g. Compute, Storage, Networking).
/// API: /api/dcim/rack-roles/
/// </summary>
[ResourceType("RackRole")]
public class RackRole : NameSlugIdentifiers
{
    [TypeProperty("Color in hex (e.g. '4caf50').", ObjectTypePropertyFlags.None)]
    public string? Color { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// A location within a site (e.g. building, floor, room, cage).
/// API: /api/dcim/locations/
/// </summary>
[ResourceType("Location")]
public class Location : NameSlugIdentifiers
{
    [TypeProperty("The site ID (required).", ObjectTypePropertyFlags.Required)]
    public required string Site { get; set; }

    [TypeProperty("The parent location ID (for nesting).", ObjectTypePropertyFlags.None)]
    public string? Parent { get; set; }

    [TypeProperty("The tenant ID.", ObjectTypePropertyFlags.None)]
    public string? Tenant { get; set; }

    [TypeProperty("Operational status: planned, staging, active, decommissioning, retired.", ObjectTypePropertyFlags.None)]
    public string? Status { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// Identifier for a Rack. Uses name as the unique key (unique per site).
/// </summary>
public class RackIdentifiers : INetboxResource
{
    [TypeProperty("Rack name (unique per site).", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required string Name { get; set; }

    [TypeProperty("NetBox internal ID (read-only, returned after create/update).", ObjectTypePropertyFlags.ReadOnly)]
    public string? Id { get; set; }

    [TypeProperty("API URL for this resource.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Url { get; set; }

    [TypeProperty("Display name.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Display { get; set; }
}

/// <summary>
/// A physical equipment rack.
/// API: /api/dcim/racks/
/// </summary>
[ResourceType("Rack")]
public class Rack : RackIdentifiers
{
    [TypeProperty("The site ID (required).", ObjectTypePropertyFlags.Required)]
    public required string Site { get; set; }

    [TypeProperty("Operational status: reserved, available, planned, active, deprecated.", ObjectTypePropertyFlags.None)]
    public string? Status { get; set; }

    [TypeProperty("The rack role ID.", ObjectTypePropertyFlags.None)]
    public string? Role { get; set; }

    [TypeProperty("The location ID (room, floor, etc.).", ObjectTypePropertyFlags.None)]
    public string? Location { get; set; }

    [TypeProperty("The tenant ID.", ObjectTypePropertyFlags.None)]
    public string? Tenant { get; set; }

    [TypeProperty("Serial number.", ObjectTypePropertyFlags.None)]
    public string? Serial { get; set; }

    [TypeProperty("Unique asset tag.", ObjectTypePropertyFlags.None)]
    public string? AssetTag { get; set; }

    [TypeProperty("Facility ID (data center reference).", ObjectTypePropertyFlags.None)]
    public string? FacilityId { get; set; }

    [TypeProperty("Height in rack units (e.g. '42').", ObjectTypePropertyFlags.None)]
    public string? UHeight { get; set; }

    [TypeProperty("Rail-to-rail width: 10, 19, 21, 23 (inches).", ObjectTypePropertyFlags.None)]
    public string? Width { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

// ──────────────────────────────────────────────
// Users: Users and Groups
// ──────────────────────────────────────────────

/// <summary>
/// Identifier for a User. Uses username as the unique key.
/// </summary>
public class UsernameIdentifiers : INetboxResource
{
    [TypeProperty("Username.", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required string Username { get; set; }

    [TypeProperty("NetBox internal ID (read-only, returned after create/update).", ObjectTypePropertyFlags.ReadOnly)]
    public string? Id { get; set; }

    [TypeProperty("API URL for this resource.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Url { get; set; }

    [TypeProperty("Display name.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Display { get; set; }
}

/// <summary>
/// A NetBox user account.
/// API: /api/users/users/
/// WARNING: Managing users via IaC requires careful handling of passwords.
/// </summary>
[ResourceType("User")]
public class User : UsernameIdentifiers
{
    [TypeProperty("Password (write-only, never returned).", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.WriteOnly)]
    public required string Password { get; set; }

    [TypeProperty("First name.", ObjectTypePropertyFlags.None)]
    public string? FirstName { get; set; }

    [TypeProperty("Last name.", ObjectTypePropertyFlags.None)]
    public string? LastName { get; set; }

    [TypeProperty("Email address.", ObjectTypePropertyFlags.None)]
    public string? Email { get; set; }

    [TypeProperty("Whether the user has staff access (true/false).", ObjectTypePropertyFlags.None)]
    public string? IsStaff { get; set; }

    [TypeProperty("Whether the user account is active (true/false).", ObjectTypePropertyFlags.None)]
    public string? IsActive { get; set; }
}

/// <summary>
/// A user group for permissions.
/// API: /api/users/groups/
/// </summary>
[ResourceType("UserGroup")]
public class UserGroup : NameIdentifiers
{
    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }
}

// ──────────────────────────────────────────────
// Circuits: CircuitTypes, Providers, Circuits
// ──────────────────────────────────────────────

/// <summary>
/// A type of circuit (e.g. Internet, MPLS, Dark Fiber).
/// API: /api/circuits/circuit-types/
/// </summary>
[ResourceType("CircuitType")]
public class CircuitType : NameSlugIdentifiers
{
    [TypeProperty("Color in hex (e.g. 'aa1409').", ObjectTypePropertyFlags.None)]
    public string? Color { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// A circuit provider (e.g. AT&T, Zayo, Lumen).
/// API: /api/circuits/providers/
/// </summary>
[ResourceType("Provider")]
public class Provider : NameSlugIdentifiers
{
    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// Identifier for a Circuit. Uses CID (Circuit ID) as the unique key.
/// </summary>
public class CidIdentifiers : INetboxResource
{
    [TypeProperty("Circuit ID (unique identifier assigned by the provider).", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required string Cid { get; set; }

    [TypeProperty("NetBox internal ID (read-only, returned after create/update).", ObjectTypePropertyFlags.ReadOnly)]
    public string? Id { get; set; }

    [TypeProperty("API URL for this resource.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Url { get; set; }

    [TypeProperty("Display name.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Display { get; set; }
}

/// <summary>
/// A communications circuit (e.g. Internet, MPLS, point-to-point).
/// API: /api/circuits/circuits/
/// </summary>
[ResourceType("Circuit")]
public class Circuit : CidIdentifiers
{
    [TypeProperty("The provider ID.", ObjectTypePropertyFlags.Required)]
    public required string Provider { get; set; }

    [TypeProperty("The circuit type ID.", ObjectTypePropertyFlags.Required)]
    public required string Type { get; set; }

    [TypeProperty("Operational status: planned, provisioning, active, offline, deprovisioned, decommissioned.", ObjectTypePropertyFlags.None)]
    public string? Status { get; set; }

    [TypeProperty("The tenant ID.", ObjectTypePropertyFlags.None)]
    public string? Tenant { get; set; }

    [TypeProperty("Date of installation (yyyy-MM-dd).", ObjectTypePropertyFlags.None)]
    public string? InstallDate { get; set; }

    [TypeProperty("Date of termination (yyyy-MM-dd).", ObjectTypePropertyFlags.None)]
    public string? TerminationDate { get; set; }

    [TypeProperty("Committed rate in Kbps.", ObjectTypePropertyFlags.None)]
    public string? CommitRate { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

// ──────────────────────────────────────────────
// DCIM additions: Regions, SiteGroups, Platforms, Interfaces, RackTypes
// ──────────────────────────────────────────────

/// <summary>
/// A geographic region (e.g. continent, country, state).
/// API: /api/dcim/regions/
/// </summary>
[ResourceType("Region")]
public class Region : NameSlugIdentifiers
{
    [TypeProperty("The parent region ID (for nesting).", ObjectTypePropertyFlags.None)]
    public string? Parent { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// A logical group of sites (e.g. corporate, branch, edge).
/// API: /api/dcim/site-groups/
/// </summary>
[ResourceType("SiteGroup")]
public class SiteGroup : NameSlugIdentifiers
{
    [TypeProperty("The parent group ID (for nesting).", ObjectTypePropertyFlags.None)]
    public string? Parent { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// A device platform (OS/firmware, e.g. Cisco IOS, Junos).
/// API: /api/dcim/platforms/
/// </summary>
[ResourceType("Platform")]
public class Platform : NameSlugIdentifiers
{
    [TypeProperty("The manufacturer ID.", ObjectTypePropertyFlags.None)]
    public string? Manufacturer { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// Identifier for an Interface. Uses name as the unique key (unique per device).
/// </summary>
public class InterfaceIdentifiers : INetboxResource
{
    [TypeProperty("Interface name (e.g. 'GigabitEthernet0/0').", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required string Name { get; set; }

    [TypeProperty("NetBox internal ID (read-only, returned after create/update).", ObjectTypePropertyFlags.ReadOnly)]
    public string? Id { get; set; }

    [TypeProperty("API URL for this resource.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Url { get; set; }

    [TypeProperty("Display name.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Display { get; set; }
}

/// <summary>
/// A physical network interface on a device.
/// API: /api/dcim/interfaces/
/// </summary>
[ResourceType("Interface")]
public class Interface : InterfaceIdentifiers
{
    [TypeProperty("The device ID.", ObjectTypePropertyFlags.Required)]
    public required string Device { get; set; }

    [TypeProperty("Interface type (e.g. '1000base-t', '10gbase-x-sfpp').", ObjectTypePropertyFlags.Required)]
    public required string Type { get; set; }

    [TypeProperty("Whether the interface is enabled (true/false).", ObjectTypePropertyFlags.None)]
    public string? Enabled { get; set; }

    [TypeProperty("MTU (1-65536).", ObjectTypePropertyFlags.None)]
    public string? Mtu { get; set; }

    [TypeProperty("Interface speed in Kbps.", ObjectTypePropertyFlags.None)]
    public string? Speed { get; set; }

    [TypeProperty("Duplex mode: half, full, auto.", ObjectTypePropertyFlags.None)]
    public string? Duplex { get; set; }

    [TypeProperty("802.1Q mode: access, tagged, tagged-all.", ObjectTypePropertyFlags.None)]
    public string? Mode { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }
}

/// <summary>
/// A rack hardware model (template for racks).
/// API: /api/dcim/rack-types/
/// </summary>
[ResourceType("RackType")]
public class RackType : SlugIdentifiers
{
    [TypeProperty("The manufacturer ID.", ObjectTypePropertyFlags.Required)]
    public required string Manufacturer { get; set; }

    [TypeProperty("Rack type model name.", ObjectTypePropertyFlags.Required)]
    public required string Model { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Height in rack units (e.g. '42').", ObjectTypePropertyFlags.None)]
    public string? UHeight { get; set; }

    [TypeProperty("Rail-to-rail width: 10, 19, 21, 23 (inches).", ObjectTypePropertyFlags.None)]
    public string? Width { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

// ──────────────────────────────────────────────
// Tenancy additions: TenantGroups, Contacts, ContactGroups, ContactRoles
// ──────────────────────────────────────────────

/// <summary>
/// A group of tenants (e.g. by department or business unit).
/// API: /api/tenancy/tenant-groups/
/// </summary>
[ResourceType("TenantGroup")]
public class TenantGroup : NameSlugIdentifiers
{
    [TypeProperty("The parent group ID (for nesting).", ObjectTypePropertyFlags.None)]
    public string? Parent { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// A contact person.
/// API: /api/tenancy/contacts/
/// </summary>
[ResourceType("Contact")]
public class Contact : NameIdentifiers
{
    [TypeProperty("Job title.", ObjectTypePropertyFlags.None)]
    public string? Title { get; set; }

    [TypeProperty("Phone number.", ObjectTypePropertyFlags.None)]
    public string? Phone { get; set; }

    [TypeProperty("Email address.", ObjectTypePropertyFlags.None)]
    public string? Email { get; set; }

    [TypeProperty("Mailing address.", ObjectTypePropertyFlags.None)]
    public string? Address { get; set; }

    [TypeProperty("URL link.", ObjectTypePropertyFlags.None)]
    public string? Link { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// A group of contacts.
/// API: /api/tenancy/contact-groups/
/// </summary>
[ResourceType("ContactGroup")]
public class ContactGroup : NameSlugIdentifiers
{
    [TypeProperty("The parent group ID (for nesting).", ObjectTypePropertyFlags.None)]
    public string? Parent { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// A functional role for contacts (e.g. NOC, billing, emergency).
/// API: /api/tenancy/contact-roles/
/// </summary>
[ResourceType("ContactRole")]
public class ContactRole : NameSlugIdentifiers
{
    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

// ──────────────────────────────────────────────
// Extras: Tags
// ──────────────────────────────────────────────

/// <summary>
/// A tag for labeling and filtering objects.
/// API: /api/extras/tags/
/// </summary>
[ResourceType("Tag")]
public class Tag : NameSlugIdentifiers
{
    [TypeProperty("Color in hex (e.g. '9e9e9e').", ObjectTypePropertyFlags.None)]
    public string? Color { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }
}

// ──────────────────────────────────────────────
// VPN: TunnelGroups, Tunnels, IKE, IPSec, L2VPN
// ──────────────────────────────────────────────

/// <summary>
/// A logical group of VPN tunnels.
/// API: /api/vpn/tunnel-groups/
/// </summary>
[ResourceType("TunnelGroup")]
public class TunnelGroup : NameSlugIdentifiers
{
    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// A VPN tunnel.
/// API: /api/vpn/tunnels/
/// </summary>
[ResourceType("Tunnel")]
public class Tunnel : NameIdentifiers
{
    [TypeProperty("Operational status: planned, active, disabled.", ObjectTypePropertyFlags.Required)]
    public required string Status { get; set; }

    [TypeProperty("Encapsulation type: ipsec-transport, ipsec-tunnel, ip-ip, gre.", ObjectTypePropertyFlags.Required)]
    public required string Encapsulation { get; set; }

    [TypeProperty("The tunnel group ID.", ObjectTypePropertyFlags.None)]
    public string? Group { get; set; }

    [TypeProperty("The IPSec profile ID.", ObjectTypePropertyFlags.None)]
    public string? IpsecProfile { get; set; }

    [TypeProperty("The tenant ID.", ObjectTypePropertyFlags.None)]
    public string? Tenant { get; set; }

    [TypeProperty("Numeric tunnel identifier.", ObjectTypePropertyFlags.None)]
    public string? TunnelId { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// An IKE (Internet Key Exchange) proposal.
/// API: /api/vpn/ike-proposals/
/// </summary>
[ResourceType("IKEProposal")]
public class IKEProposal : NameIdentifiers
{
    [TypeProperty("Authentication method (e.g. preshared-keys, certificates, rsa-signatures).", ObjectTypePropertyFlags.Required)]
    public required string AuthenticationMethod { get; set; }

    [TypeProperty("Encryption algorithm (e.g. aes-128-cbc, aes-256-cbc, 3des-cbc).", ObjectTypePropertyFlags.Required)]
    public required string EncryptionAlgorithm { get; set; }

    [TypeProperty("Authentication/hash algorithm (e.g. hmac-sha1, hmac-sha256, hmac-md5).", ObjectTypePropertyFlags.None)]
    public string? AuthenticationAlgorithm { get; set; }

    [TypeProperty("Diffie-Hellman group (e.g. 1, 2, 5, 14, 19, 20).", ObjectTypePropertyFlags.Required)]
    public required string Group { get; set; }

    [TypeProperty("SA lifetime in seconds.", ObjectTypePropertyFlags.None)]
    public string? SaLifetime { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// An IKE (Internet Key Exchange) policy.
/// API: /api/vpn/ike-policies/
/// </summary>
[ResourceType("IKEPolicy")]
public class IKEPolicy : NameIdentifiers
{
    [TypeProperty("IKE version: 1, 2.", ObjectTypePropertyFlags.Required)]
    public required string Version { get; set; }

    [TypeProperty("IKE mode: main, aggressive.", ObjectTypePropertyFlags.None)]
    public string? Mode { get; set; }

    [TypeProperty("Pre-shared key.", ObjectTypePropertyFlags.None)]
    public string? PresharedKey { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// An IPSec proposal (transform set).
/// API: /api/vpn/ipsec-proposals/
/// </summary>
[ResourceType("IPSecProposal")]
public class IPSecProposal : NameIdentifiers
{
    [TypeProperty("Encryption algorithm (e.g. aes-128-cbc, aes-256-cbc, des-cbc).", ObjectTypePropertyFlags.None)]
    public string? EncryptionAlgorithm { get; set; }

    [TypeProperty("Authentication/hash algorithm (e.g. hmac-sha1, hmac-sha256, hmac-md5).", ObjectTypePropertyFlags.None)]
    public string? AuthenticationAlgorithm { get; set; }

    [TypeProperty("SA lifetime in seconds.", ObjectTypePropertyFlags.None)]
    public string? SaLifetimeSeconds { get; set; }

    [TypeProperty("SA lifetime in kilobytes of data.", ObjectTypePropertyFlags.None)]
    public string? SaLifetimeData { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// An IPSec policy (grouping of IPSec proposals).
/// API: /api/vpn/ipsec-policies/
/// </summary>
[ResourceType("IPSecPolicy")]
public class IPSecPolicy : NameIdentifiers
{
    [TypeProperty("Perfect Forward Secrecy group (e.g. 1, 2, 5, 14, 19, 20).", ObjectTypePropertyFlags.None)]
    public string? PfsGroup { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// An IPSec profile (links IKE policy and IPSec policy).
/// API: /api/vpn/ipsec-profiles/
/// </summary>
[ResourceType("IPSecProfile")]
public class IPSecProfile : NameIdentifiers
{
    [TypeProperty("IPSec mode: esp, ah.", ObjectTypePropertyFlags.Required)]
    public required string Mode { get; set; }

    [TypeProperty("The IKE policy ID.", ObjectTypePropertyFlags.Required)]
    public required string IkePolicy { get; set; }

    [TypeProperty("The IPSec policy ID.", ObjectTypePropertyFlags.Required)]
    public required string IpsecPolicy { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// A Layer 2 VPN (e.g. VPLS, VXLAN, EVPN).
/// API: /api/vpn/l2vpns/
/// </summary>
[ResourceType("L2VPN")]
public class L2VPN : NameSlugIdentifiers
{
    [TypeProperty("L2VPN type (e.g. vpls, vxlan, vpws, ep-lan, evp-lan, ep-tree).", ObjectTypePropertyFlags.None)]
    public string? Type { get; set; }

    [TypeProperty("Operational status (e.g. active, inactive).", ObjectTypePropertyFlags.None)]
    public string? Status { get; set; }

    [TypeProperty("Numeric L2VPN identifier (VNI, VXLAN ID, etc.).", ObjectTypePropertyFlags.None)]
    public string? Identifier { get; set; }

    [TypeProperty("The tenant ID.", ObjectTypePropertyFlags.None)]
    public string? Tenant { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

// ──────────────────────────────────────────────
// Wireless: WirelessLANGroups, WirelessLANs, WirelessLinks
// ──────────────────────────────────────────────

/// <summary>
/// A logical group of wireless LANs.
/// API: /api/wireless/wireless-lan-groups/
/// </summary>
[ResourceType("WirelessLANGroup")]
public class WirelessLANGroup : NameSlugIdentifiers
{
    [TypeProperty("The parent group ID (for nesting).", ObjectTypePropertyFlags.None)]
    public string? Parent { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// Identifier for a Wireless LAN. Uses SSID as the unique key.
/// </summary>
public class SsidIdentifiers : INetboxResource
{
    [TypeProperty("Service Set Identifier (SSID).", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required string Ssid { get; set; }

    [TypeProperty("NetBox internal ID (read-only, returned after create/update).", ObjectTypePropertyFlags.ReadOnly)]
    public string? Id { get; set; }

    [TypeProperty("API URL for this resource.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Url { get; set; }

    [TypeProperty("Display name.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Display { get; set; }
}

/// <summary>
/// A wireless LAN (Wi-Fi network).
/// API: /api/wireless/wireless-lans/
/// </summary>
[ResourceType("WirelessLAN")]
public class WirelessLAN : SsidIdentifiers
{
    [TypeProperty("The wireless LAN group ID.", ObjectTypePropertyFlags.None)]
    public string? Group { get; set; }

    [TypeProperty("Operational status: active, reserved, disabled, deprecated.", ObjectTypePropertyFlags.None)]
    public string? Status { get; set; }

    [TypeProperty("The VLAN ID.", ObjectTypePropertyFlags.None)]
    public string? Vlan { get; set; }

    [TypeProperty("The tenant ID.", ObjectTypePropertyFlags.None)]
    public string? Tenant { get; set; }

    [TypeProperty("Authentication type: open, wep, wpa-personal, wpa-enterprise.", ObjectTypePropertyFlags.None)]
    public string? AuthType { get; set; }

    [TypeProperty("Authentication cipher: auto, tkip, aes.", ObjectTypePropertyFlags.None)]
    public string? AuthCipher { get; set; }

    [TypeProperty("Pre-shared key.", ObjectTypePropertyFlags.None)]
    public string? AuthPsk { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// Identifier for a Wireless Link. Uses interface A as the unique key.
/// </summary>
public class WirelessLinkIdentifiers : INetboxResource
{
    [TypeProperty("The first interface ID (side A).", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required string InterfaceA { get; set; }

    [TypeProperty("NetBox internal ID (read-only, returned after create/update).", ObjectTypePropertyFlags.ReadOnly)]
    public string? Id { get; set; }

    [TypeProperty("API URL for this resource.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Url { get; set; }

    [TypeProperty("Display name.", ObjectTypePropertyFlags.ReadOnly)]
    public string? Display { get; set; }
}

/// <summary>
/// A point-to-point wireless link between two interfaces.
/// API: /api/wireless/wireless-links/
/// </summary>
[ResourceType("WirelessLink")]
public class WirelessLink : WirelessLinkIdentifiers
{
    [TypeProperty("The second interface ID (side B).", ObjectTypePropertyFlags.Required)]
    public required string InterfaceB { get; set; }

    [TypeProperty("SSID for the wireless link.", ObjectTypePropertyFlags.None)]
    public string? Ssid { get; set; }

    [TypeProperty("Operational status (e.g. connected, planned, decommissioning).", ObjectTypePropertyFlags.None)]
    public string? Status { get; set; }

    [TypeProperty("The tenant ID.", ObjectTypePropertyFlags.None)]
    public string? Tenant { get; set; }

    [TypeProperty("Authentication type: open, wep, wpa-personal, wpa-enterprise.", ObjectTypePropertyFlags.None)]
    public string? AuthType { get; set; }

    [TypeProperty("Authentication cipher: auto, tkip, aes.", ObjectTypePropertyFlags.None)]
    public string? AuthCipher { get; set; }

    [TypeProperty("Pre-shared key.", ObjectTypePropertyFlags.None)]
    public string? AuthPsk { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}
