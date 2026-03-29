using System.Text.Json.Serialization;
using Azure.Bicep.Types.Concrete;
using Bicep.Local.Extension.Types.Attributes;

namespace Bicep.Extension.Netbox;

/// <summary>
/// Extension-level configuration passed via: extension netbox with { url: '...', token: '...' }
/// </summary>
public class Configuration
{
    [TypeProperty("The base URL of the NetBox instance (e.g. https://netbox.example.com).", ObjectTypePropertyFlags.Required)]
    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [TypeProperty("The NetBox API token for authentication.", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.WriteOnly)]
    [JsonPropertyName("token")]
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
    [JsonPropertyName("slug")]
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
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [TypeProperty("Operational status: planned, staging, active, decommissioning, retired.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [TypeProperty("Local facility ID or description.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("facility")]
    public string? Facility { get; set; }

    [TypeProperty("A brief description of the site.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [TypeProperty("Physical address of the site.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("physical_address")]
    public string? PhysicalAddress { get; set; }

    [TypeProperty("Shipping address (if different from physical).", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("shipping_address")]
    public string? ShippingAddress { get; set; }

    [TypeProperty("IANA time zone (e.g. 'America/New_York').", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("time_zone")]
    public string? TimeZone { get; set; }

    [TypeProperty("GPS latitude (-90 to 90) as a string (e.g. '59.3293').", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("latitude")]
    public string? Latitude { get; set; }

    [TypeProperty("GPS longitude (-180 to 180) as a string (e.g. '18.0686').", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("longitude")]
    public string? Longitude { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("comments")]
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
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("comments")]
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
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [TypeProperty("Color in hex (e.g. 'aa1409').", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("color")]
    public string? Color { get; set; }

    [TypeProperty("Whether this role can be assigned to virtual machines (true/false).", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("vm_role")]
    public string? VmRole { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("comments")]
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
    [JsonPropertyName("manufacturer")]
    public required int Manufacturer { get; set; }

    [TypeProperty("Model name.", ObjectTypePropertyFlags.Required)]
    [JsonPropertyName("model")]
    public required string Model { get; set; }

    [TypeProperty("Part number.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("part_number")]
    public string? PartNumber { get; set; }

    [TypeProperty("Height in rack units.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("u_height")]
    public string? UHeight { get; set; }

    [TypeProperty("Whether the device takes up the full depth of a rack (true/false).", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("is_full_depth")]
    public string? IsFullDepth { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("comments")]
    public string? Comments { get; set; }
}

/// <summary>
/// Identifier for a Device. Uses name as the unique key (unique per site).
/// </summary>
public class DeviceIdentifiers
{
    [TypeProperty("Device name (unique per site).", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    [JsonPropertyName("name")]
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
    [JsonPropertyName("device_type")]
    public required int DeviceType { get; set; }

    [TypeProperty("The device role ID.", ObjectTypePropertyFlags.Required)]
    [JsonPropertyName("role")]
    public required int Role { get; set; }

    [TypeProperty("The site ID where this device is located.", ObjectTypePropertyFlags.Required)]
    [JsonPropertyName("site")]
    public required int Site { get; set; }

    [TypeProperty("Operational status: offline, active, planned, staged, failed, inventory, decommissioning.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [TypeProperty("The rack ID.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("rack")]
    public string? Rack { get; set; }

    [TypeProperty("Position in rack (starting from bottom).", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("position")]
    public string? Position { get; set; }

    [TypeProperty("Rack face: front, rear.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("face")]
    public string? Face { get; set; }

    [TypeProperty("The tenant ID.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("tenant")]
    public string? Tenant { get; set; }

    [TypeProperty("The platform ID (OS/firmware).", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("platform")]
    public string? Platform { get; set; }

    [TypeProperty("Serial number.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("serial")]
    public string? Serial { get; set; }

    [TypeProperty("Unique asset tag.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("asset_tag")]
    public string? AssetTag { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("comments")]
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
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [TypeProperty("The tenant group ID.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("group")]
    public string? Group { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("comments")]
    public string? Comments { get; set; }
}

// ──────────────────────────────────────────────
// IPAM: Prefixes, IP Addresses, VLANs
// ──────────────────────────────────────────────

/// <summary>
/// Identifier for a Prefix. Uses the CIDR notation as the unique key.
/// </summary>
public class PrefixIdentifiers
{
    [TypeProperty("IP prefix in CIDR notation (e.g. '10.0.0.0/24').", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    [JsonPropertyName("prefix")]
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
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [TypeProperty("The VRF ID.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("vrf")]
    public string? Vrf { get; set; }

    [TypeProperty("The tenant ID.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("tenant")]
    public string? Tenant { get; set; }

    [TypeProperty("The VLAN ID.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("vlan")]
    public string? Vlan { get; set; }

    [TypeProperty("The IPAM role ID.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("role")]
    public string? Role { get; set; }

    [TypeProperty("Treat this prefix as a pool of available addresses (true/false).", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("is_pool")]
    public string? IsPool { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("comments")]
    public string? Comments { get; set; }
}

/// <summary>
/// Identifier for an IP Address. Uses CIDR notation as the unique key.
/// </summary>
public class IPAddressIdentifiers
{
    [TypeProperty("IP address in CIDR notation (e.g. '10.0.0.1/24').", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    [JsonPropertyName("address")]
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
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [TypeProperty("Functional role: loopback, secondary, anycast, vip, vrrp, hsrp, glbp, carp.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("role")]
    public string? Role { get; set; }

    [TypeProperty("The VRF ID.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("vrf")]
    public string? Vrf { get; set; }

    [TypeProperty("The tenant ID.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("tenant")]
    public string? Tenant { get; set; }

    [TypeProperty("DNS hostname.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("dns_name")]
    public string? DnsName { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("comments")]
    public string? Comments { get; set; }
}

/// <summary>
/// Identifier for a VLAN. Uses vid (VLAN ID number) as the unique key.
/// </summary>
public class VLANIdentifiers
{
    [TypeProperty("VLAN ID number (1-4094).", ObjectTypePropertyFlags.Required | ObjectTypePropertyFlags.Identifier)]
    [JsonPropertyName("vid")]
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
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [TypeProperty("Operational status: active, reserved, deprecated.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [TypeProperty("The VLAN group ID.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("group")]
    public string? Group { get; set; }

    [TypeProperty("The site ID.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("site")]
    public string? Site { get; set; }

    [TypeProperty("The tenant ID.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("tenant")]
    public string? Tenant { get; set; }

    [TypeProperty("The IPAM role ID.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("role")]
    public string? Role { get; set; }

    [TypeProperty("A brief description.", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [TypeProperty("Free-text comments (markdown supported).", ObjectTypePropertyFlags.None)]
    [JsonPropertyName("comments")]
    public string? Comments { get; set; }
}
