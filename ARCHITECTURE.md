# bicep-ext-netbox

A Bicep extension for managing [NetBox](https://netboxlabs.com/products/netbox/) resources using the Bicep language.

## Overview

This extension allows you to declare NetBox resources (sites, devices, IP addresses, etc.) in Bicep and deploy them via `bicep local-deploy`. It communicates with the NetBox REST API under the hood.

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Bicep CLI](https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/install) v0.37.4 or later
- A NetBox instance with API access and a valid API token
- Optionally, run NetBox locally via [netbox-docker](https://github.com/netbox-community/netbox-docker)

## Supported Resource Types

### DCIM (Data Center Infrastructure Management)

| Resource | Bicep Type | NetBox API | Identifier | FK Dependencies |
|----------|-----------|-----------|------------|-----------------|
| Site | `Site` | `/api/dcim/sites/` | `slug` | None |
| Manufacturer | `Manufacturer` | `/api/dcim/manufacturers/` | `slug` | None |
| Device Role | `DeviceRole` | `/api/dcim/device-roles/` | `slug` | None |
| Device Type | `DeviceType` | `/api/dcim/device-types/` | `slug` | Manufacturer (ID) |
| Device | `Device` | `/api/dcim/devices/` | `name` | DeviceType, DeviceRole, Site (IDs) |

### Tenancy

| Resource | Bicep Type | NetBox API | Identifier | FK Dependencies |
|----------|-----------|-----------|------------|-----------------|
| Tenant | `Tenant` | `/api/tenancy/tenants/` | `slug` | None |

### IPAM (IP Address Management)

| Resource | Bicep Type | NetBox API | Identifier | FK Dependencies |
|----------|-----------|-----------|------------|-----------------|
| Prefix | `Prefix` | `/api/ipam/prefixes/` | `prefix` (CIDR) | None required |
| IP Address | `IPAddress` | `/api/ipam/ip-addresses/` | `address` (CIDR) | None required |
| VLAN | `VLAN` | `/api/ipam/vlans/` | `vid` | None required |

## Quick Start

### 1. Build and publish the extension locally

```powershell
./scripts/publish.ps1
```

### 2. Set environment variables

```powershell
$env:NETBOX_URL = "http://localhost:8000"   # or your NetBox instance URL
$env:NETBOX_TOKEN = "your-api-token-here"
```

### 3. Deploy

```powershell
bicep local-deploy ./samples/basic/main.bicepparam
```

## Usage Example

```bicep
targetScope = 'local'

@secure()
param netboxToken string
param netboxUrl string

extension netbox with {
  url: netboxUrl
  token: netboxToken
}

// Tenant
resource tenant 'Tenant' = {
  name: 'Contoso'
  slug: 'contoso'
  description: 'Contoso Corporation'
}

// Site
resource site 'Site' = {
  name: 'Stockholm DC1'
  slug: 'stockholm-dc1'
  status: 'active'
  description: 'Primary datacenter in Stockholm'
}

// Hardware catalog
resource manufacturer 'Manufacturer' = {
  name: 'Cisco'
  slug: 'cisco'
}

resource deviceRole 'DeviceRole' = {
  name: 'Router'
  slug: 'router'
  color: 'aa1409'
}

// IPAM
resource prefix 'Prefix' = {
  prefix: '10.0.0.0/24'
  status: 'active'
  description: 'Management network'
}

resource vlan 'VLAN' = {
  vid: 100
  name: 'Management'
  status: 'active'
}
```

## Project Structure

```
src/
  Program.cs                          # Entry point — registers extension and handlers
  Models.cs                           # Resource type definitions (C# classes with attributes)
  Handlers/
    NetboxResourceHandlerBase.cs      # Shared HTTP client, auth, and error handling
    SiteHandler.cs                    # ~10 lines each — only API path + lookup + identifiers
    ManufacturerHandler.cs
    DeviceRoleHandler.cs
    DeviceTypeHandler.cs
    DeviceHandler.cs
    TenantHandler.cs
    PrefixHandler.cs
    IPAddressHandler.cs
    VLANHandler.cs
samples/
  basic/                              # Example Bicep templates
scripts/
  publish.ps1                         # Build and package script
```

## Architecture Decisions

- **C# / .NET 9** — Required by the Bicep extensibility SDK (`Azure.Bicep.Local.Extension`). There is no alternative SDK for other languages.
- **HttpClient directly** — No third-party NetBox client library; the REST API is straightforward and this avoids an extra dependency.
- **Base handler pattern** — `NetboxResourceHandlerBase<T, TId>` provides shared HTTP setup, authentication, lookup, create-or-update, and error handling. Each resource handler is ~10 lines specifying only the API path, lookup query, and identifier mapping.
- **Shared `SlugIdentifiers`** — Most NetBox resources use `slug` as their unique key. A single shared identifier class eliminates duplication across Site, Tenant, Manufacturer, DeviceRole, and DeviceType.
- **Flexible lookup queries** — The base handler's `GetLookupQuery()` method supports different lookup strategies per resource: `slug=` for slug-based resources, `name=` for devices, `prefix=` for CIDR-based, `vid=` for VLANs.
- **FK references as integer IDs** — Foreign key fields (e.g., `device_type`, `site`) accept NetBox integer IDs. This is the simplest approach; slug-based FK resolution may be added later.

## What-If Support

**Current status:** The `Preview()` handler method is implemented and returns the desired resource state without making API calls (matching all official sample extensions). However, the Bicep CLI (`bicep local-deploy`) does not currently expose a `--what-if` flag for local extensions. The `Preview()` method is used internally by the SDK during `bicep build` for type validation only.

**Limitation:** This is a limitation of the experimental Bicep extensibility SDK, not this extension. There is no way for users to run a what-if/dry-run deployment against NetBox with the current Bicep CLI.

**What works today:**
- `bicep build` validates your Bicep file against the resource type schemas (property names, required fields, types)
- Deployments are idempotent — running twice creates then updates, never duplicates

**What doesn't work yet:**
- No `--what-if` CLI flag for `bicep local-deploy`
- No create-vs-update diff output before deploying
- No delete detection (resources removed from Bicep are not removed from NetBox)

## Known Limitations

### Resources not supported via REST API

Some NetBox features are only configurable through the web UI or `configuration.py` and have no REST API endpoints:

| Feature | Why | Workaround |
|---------|-----|------------|
| **Landing page banners** (`BANNER_TOP`, `BANNER_BOTTOM`, `BANNER_LOGIN`) | Stored in `ConfigRevision` model with no API viewset | Set via web UI (Admin > Configuration) or hardcode in `configuration.py` |
| **System settings** (maintenance mode, pagination, login requirements) | Same `ConfigRevision` system — UI only | Same as above |

This is a NetBox limitation — if the NetBox project adds a REST API for `ConfigRevision` in the future, we can add Bicep support for these settings.

### Bicep SDK type limitations

The `Azure.Bicep.Local.Extension` SDK does not support nullable value types:
- No `int?`, `bool?`, or `double?` — optional numeric/boolean fields use `string?` instead
- No array types (`int[]`, `string[]`) — resources requiring arrays (ServiceTemplate, Service, FHRP Group Assignment) are not yet supported

### No delete support

Removing a resource from your Bicep file and redeploying will **not** delete it from NetBox. The extension only creates and updates. This is the safer default and matches the behavior of all official sample Bicep extensions.

## Adding New Resource Types

To add a new NetBox resource type:

1. Add model classes in `Models.cs` (identifiers class + resource class with `[ResourceType]` attribute)
2. Create a handler in `Handlers/` inheriting from `NetboxResourceHandlerBase<T, TId>` (~10 lines)
3. Register the handler in `Program.cs` with `.WithResourceHandler<YourHandler>()`

## License

MIT
