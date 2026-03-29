using Azure.Bicep.Types.Concrete;
using Bicep.Local.Extension.Types.Attributes;

namespace Bicep.Extension.Netbox;

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
// Shared base: many NetBox resources use slug as their unique identifier
// ──────────────────────────────────────────────

/// <summary>
/// Shared identifier for resources that use a slug as their unique key.
/// Used by: Site, Tenant, Manufacturer, DeviceRole, DeviceType.
/// </summary>
public class SlugIdentifiers
{
    [TypeProperty("URL-friendly unique identifier (e.g. 'my-datacenter').", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required string Slug { get; set; }
}

// ──────────────────────────────────────────────
// DCIM: Sites, Manufacturers, DeviceRoles, DeviceTypes, Devices
// ──────────────────────────────────────────────

/// <summary>
/// A physical location (datacenter, office, etc.).
/// API: /api/dcim/sites/
/// </summary>
[ResourceType("Site")]
public class Site : SlugIdentifiers
{
    [TypeProperty("Full name of the site.", ObjectTypePropertyFlags.Required)]
    public required string Name { get; set; }

    [TypeProperty("Operational status: planned, staging, active, decommissioning, retired.", ObjectTypePropertyFlags.None)]
    public string? Status { get; set; }

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
public class Manufacturer : SlugIdentifiers
{
    [TypeProperty("Manufacturer name.", ObjectTypePropertyFlags.Required)]
    public required string Name { get; set; }

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
public class DeviceRole : SlugIdentifiers
{
    [TypeProperty("Role name.", ObjectTypePropertyFlags.Required)]
    public required string Name { get; set; }

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
    public required int Manufacturer { get; set; }

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
public class DeviceIdentifiers
{
    [TypeProperty("Device name (unique per site).", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required string Name { get; set; }
}

/// <summary>
/// A physical device (server, switch, router, etc.).
/// API: /api/dcim/devices/
/// </summary>
[ResourceType("Device")]
public class Device : DeviceIdentifiers
{
    [TypeProperty("The device type ID (hardware model).", ObjectTypePropertyFlags.Required)]
    public required int DeviceType { get; set; }

    [TypeProperty("The device role ID.", ObjectTypePropertyFlags.Required)]
    public required int Role { get; set; }

    [TypeProperty("The site ID where this device is located.", ObjectTypePropertyFlags.Required)]
    public required int Site { get; set; }

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
public class Tenant : SlugIdentifiers
{
    [TypeProperty("Tenant name.", ObjectTypePropertyFlags.Required)]
    public required string Name { get; set; }

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
public class NameIdentifiers
{
    [TypeProperty("Unique name.", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required string Name { get; set; }
}

// ──────────────────────────────────────────────
// IPAM: Prefixes, IP Addresses, VLANs, VRFs, and more
// ──────────────────────────────────────────────

/// <summary>
/// Identifier for a Prefix. Uses the CIDR notation as the unique key.
/// </summary>
public class PrefixIdentifiers
{
    [TypeProperty("IP prefix in CIDR notation (e.g. '10.0.0.0/24').", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required string Prefix { get; set; }
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
public class IPAddressIdentifiers
{
    [TypeProperty("IP address in CIDR notation (e.g. '10.0.0.1/24').", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required string Address { get; set; }
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

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    public string? Comments { get; set; }
}

/// <summary>
/// Identifier for a VLAN. Uses vid (VLAN ID number) as the unique key.
/// </summary>
public class VLANIdentifiers
{
    [TypeProperty("VLAN ID number (1-4094).", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required int Vid { get; set; }
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
public class RIR : SlugIdentifiers
{
    [TypeProperty("RIR name.", ObjectTypePropertyFlags.Required)]
    public required string Name { get; set; }

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
public class AggregateIdentifiers
{
    [TypeProperty("Aggregate prefix in CIDR notation (e.g. '10.0.0.0/8').", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required string Prefix { get; set; }
}

/// <summary>
/// A top-level IP aggregate (allocated by a RIR).
/// API: /api/ipam/aggregates/
/// </summary>
[ResourceType("Aggregate")]
public class Aggregate : AggregateIdentifiers
{
    [TypeProperty("The RIR ID.", ObjectTypePropertyFlags.Required)]
    public required int Rir { get; set; }

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
public class IPAMRole : SlugIdentifiers
{
    [TypeProperty("Role name.", ObjectTypePropertyFlags.Required)]
    public required string Name { get; set; }

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
public class IPRangeIdentifiers
{
    [TypeProperty("Start IP address in CIDR notation (e.g. '10.0.0.100/24').", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required string StartAddress { get; set; }
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
public class ASNIdentifiers
{
    [TypeProperty("Autonomous System Number (e.g. 65000).", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    public required int Asn { get; set; }
}

/// <summary>
/// An Autonomous System Number.
/// API: /api/ipam/asns/
/// </summary>
[ResourceType("ASN")]
public class ASN : ASNIdentifiers
{
    [TypeProperty("The RIR ID (required in NetBox v4.5+).", ObjectTypePropertyFlags.Required)]
    public required int Rir { get; set; }

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
public class ASNRange : SlugIdentifiers
{
    [TypeProperty("Range name.", ObjectTypePropertyFlags.Required)]
    public required string Name { get; set; }

    [TypeProperty("The RIR ID.", ObjectTypePropertyFlags.Required)]
    public required int Rir { get; set; }

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
public class VLANGroup : SlugIdentifiers
{
    [TypeProperty("Group name.", ObjectTypePropertyFlags.Required)]
    public required string Name { get; set; }

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
