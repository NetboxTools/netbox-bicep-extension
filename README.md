# bicep-ext-netbox

A Bicep extension for managing [NetBox](https://netboxlabs.com/products/netbox/) resources using the Bicep language. Declare your datacenter infrastructure — sites, devices, IP addresses, VLANs — in Bicep and deploy them to NetBox.

## Getting Started

### Prerequisites

| Tool | Install | Purpose |
|------|---------|---------|
| [Bicep CLI](https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/install) | `winget install Microsoft.Bicep` | Package and deploy |
| A NetBox instance | See [Running NetBox locally](#running-netbox-locally) | Target API |

### Option A: Download a release (recommended)

1. Download the latest release for your platform from [GitHub Releases](../../releases)
2. Extract the archive
3. Package it for Bicep:

```powershell
bicep publish-extension `
  --bin-win-x64 ./bicep-ext-netbox.exe `
  --target ./bicep-ext-netbox `
  --force
```

4. Point your `bicepconfig.json` to the packaged extension:

```json
{
  "experimentalFeaturesEnabled": { "localDeploy": true, "extensibility": true },
  "extensions": { "netbox": "./bicep-ext-netbox" }
}
```

### Option B: Build from source

Requires [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (`winget install Microsoft.DotNet.SDK.9`).

```powershell
git clone https://github.com/NetboxTools/netbox-bicep-extension.git
cd netbox-bicep-extension
dotnet build bicep-ext-netbox.sln

# Publish and package
dotnet publish src/bicep-ext-netbox.csproj --configuration Release -r win-x64 -o ./extension-publish/win-x64
bicep publish-extension `
  --bin-win-x64 ./extension-publish/win-x64/bicep-ext-netbox.exe `
  --target ./extension-publish/bicep-ext-netbox `
  --force
```

Or use the cross-platform script: `./scripts/publish.ps1`

### Configure your Bicep project

After installing the extension (Option A or B above), you need three files in your project folder:

**1. `bicepconfig.json`** — tells Bicep where to find the extension:

```json
{
  "experimentalFeaturesEnabled": {
    "localDeploy": true,
    "extensibility": true
  },
  "extensions": {
    "netbox": "./path/to/bicep-ext-netbox"
  }
}
```

> Update the path to point to wherever you packaged the extension. If you built from source, this is typically `../extension-publish/bicep-ext-netbox` relative to your Bicep files.

**2. `main.bicep`** — your infrastructure declaration:

```bicep
targetScope = 'local'

@secure()
param netboxToken string
param netboxUrl string

extension netbox with {
  url: netboxUrl
  token: netboxToken
}

resource site 'Site' = {
  name: 'Stockholm DC1'
  slug: 'stockholm-dc1'
  status: 'active'
  description: 'Primary datacenter'
}

resource vlan 'VLAN' = {
  vid: 100
  name: 'Management'
  status: 'active'
}

resource prefix 'Prefix' = {
  prefix: '10.0.0.0/24'
  status: 'active'
  description: 'Management network'
}
```

Key points:
- `targetScope = 'local'` is required — this is a local extension, not an ARM deployment
- `@secure()` on the token parameter prevents it from being logged
- `extension netbox with { ... }` passes the URL and token to the extension

**3. `main.bicepparam`** — parameters file that reads credentials from environment variables:

```bicep
using 'main.bicep'

param netboxUrl = readEnvironmentVariable('NETBOX_URL')
param netboxToken = readEnvironmentVariable('NETBOX_TOKEN')
```

### Deploy

Set your environment variables and run:

```powershell
$env:NETBOX_URL = "http://localhost:8000"
$env:NETBOX_TOKEN = "nbt_your-token-here"

bicep local-deploy main.bicepparam
```

You should see:

```
╭──────────┬──────────┬───────────╮
│ Resource │ Duration │ Status    │
├──────────┼──────────┼───────────┤
│ site     │ 0,4s     │ Succeeded │
│ vlan     │ 0,5s     │ Succeeded │
│ prefix   │ 0,5s     │ Succeeded │
╰──────────┴──────────┴───────────╯
```

Running the same command again is safe — existing resources are updated, not duplicated.

## Auto-Completion / IntelliSense

The [Bicep VS Code extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-bicep) automatically provides IntelliSense for all resource types once the extension is referenced in your `bicepconfig.json`. You get:

- Auto-complete for property names
- Type validation and error highlighting
- Hover descriptions for each property
- Required vs optional field indicators

## Supported Resource Types

### DCIM

| Bicep Type | NetBox API | Identifier | Description |
|-----------|-----------|------------|-------------|
| `Site` | `/api/dcim/sites/` | `slug` | Physical locations |
| `Manufacturer` | `/api/dcim/manufacturers/` | `slug` | Hardware manufacturers |
| `DeviceRole` | `/api/dcim/device-roles/` | `slug` | Functional roles (router, switch, etc.) |
| `DeviceType` | `/api/dcim/device-types/` | `slug` | Hardware models |
| `Device` | `/api/dcim/devices/` | `name` | Physical devices |
| `RackRole` | `/api/dcim/rack-roles/` | `slug` | Functional roles for racks |
| `Location` | `/api/dcim/locations/` | `slug` | Locations within a site (room, floor) |
| `Rack` | `/api/dcim/racks/` | `name` | Physical equipment racks |

### Tenancy

| Bicep Type | NetBox API | Identifier | Description |
|-----------|-----------|------------|-------------|
| `Tenant` | `/api/tenancy/tenants/` | `slug` | Customers / business units |

### Virtualization (complete)

| Bicep Type | NetBox API | Identifier | Description |
|-----------|-----------|------------|-------------|
| `ClusterType` | `/api/virtualization/cluster-types/` | `slug` | Cluster technology (VMware, Hyper-V, Azure) |
| `ClusterGroup` | `/api/virtualization/cluster-groups/` | `slug` | Logical groups of clusters |
| `Cluster` | `/api/virtualization/clusters/` | `name` | Virtualization clusters |
| `VirtualMachine` | `/api/virtualization/virtual-machines/` | `name` | Virtual machines |
| `VMInterface` | `/api/virtualization/interfaces/` | `name` | VM network interfaces |
| `VirtualDisk` | `/api/virtualization/virtual-disks/` | `name` | VM virtual disks |

### IPAM (complete)

| Bicep Type | NetBox API | Identifier | Description |
|-----------|-----------|------------|-------------|
| `Prefix` | `/api/ipam/prefixes/` | `prefix` | IP subnets (CIDR) |
| `IPAddress` | `/api/ipam/ip-addresses/` | `address` | Individual IPs (CIDR) |
| `IPRange` | `/api/ipam/ip-ranges/` | `startAddress` | IP address ranges |
| `VLAN` | `/api/ipam/vlans/` | `vid` | VLANs |
| `VLANGroup` | `/api/ipam/vlan-groups/` | `slug` | VLAN groups |
| `VLANTranslationPolicy` | `/api/ipam/vlan-translation-policies/` | `name` | VLAN translation policies |
| `VRF` | `/api/ipam/vrfs/` | `name` | Virtual routing instances |
| `RouteTarget` | `/api/ipam/route-targets/` | `name` | BGP route targets |
| `RIR` | `/api/ipam/rirs/` | `slug` | Regional Internet Registries |
| `Aggregate` | `/api/ipam/aggregates/` | `prefix` | Top-level IP aggregates |
| `IPAMRole` | `/api/ipam/roles/` | `slug` | Prefix/VLAN functional roles |
| `ASN` | `/api/ipam/asns/` | `asn` | Autonomous System Numbers |
| `ASNRange` | `/api/ipam/asn-ranges/` | `slug` | ASN ranges |

### Users

| Bicep Type | NetBox API | Identifier | Description |
|-----------|-----------|------------|-------------|
| `User` | `/api/users/users/` | `username` | User accounts |
| `UserGroup` | `/api/users/groups/` | `name` | User groups for permissions |

## Sample Deployments

| Sample | Description | Resources |
|--------|-------------|-----------|
| [basic](samples/basic/) | Single site | 1 resource |
| [full](samples/full/) | Multi-type overview | 7 resources |
| [datacenter](samples/datacenter/) | Tenant, 2 sites, manufacturers, roles | 10 resources |
| [ipam](samples/ipam/) | VLANs, prefixes, IP addresses | 11 resources |
| [ipam-full](samples/ipam-full/) | All IPAM types: VRFs, RIRs, ASNs, etc. | 13 resources |
| [virtualization](samples/virtualization/) | Cluster types, clusters, VMs | 9 resources |
| [racks-and-users](samples/racks-and-users/) | Racks, locations, users, groups | 10 resources |

Deploy any sample:

```powershell
bicep local-deploy ./samples/<name>/main.bicepparam
```

## Running NetBox Locally

You need Docker Engine (not Docker Desktop — no license required).

### Using WSL2 + Docker Engine

```bash
# In WSL2 Ubuntu
sudo apt update && sudo apt install -y docker.io docker-compose-v2
sudo service docker start

git clone https://github.com/netbox-community/netbox-docker.git
cd netbox-docker
cp docker-compose.override.yml.example docker-compose.override.yml
docker compose up -d
```

Wait for the `netbox` container to be healthy:

```bash
docker compose ps
```

### Create a superuser and API token

```bash
# Create admin user
docker compose exec netbox /opt/netbox/netbox/manage.py createsuperuser

# Generate API token (v2 format)
docker compose exec netbox /opt/netbox/netbox/manage.py shell -c \
  "from users.models import Token, User; t = Token(user=User.objects.first()); t.save(); print(t.key)"
```

The token will look like `nbt_xxxx.yyyy`. Use it with `Bearer` auth (the extension auto-detects v1 vs v2 tokens).

### Access

- **Web UI**: http://localhost:8000
- **API**: http://localhost:8000/api/
- **Swagger**: http://localhost:8000/api/schema/swagger-ui/

## Authentication

The extension supports both NetBox token formats:

| Token format | Auth header | NetBox version |
|-------------|-------------|----------------|
| `nbt_xxxx.yyyy` | `Bearer` | v4+ (recommended) |
| `abc123...` | `Token` | v3 and earlier |

The format is auto-detected based on the `nbt_` prefix.

## Idempotent Deployments

Running the same deployment twice is safe. The extension:

1. Looks up the resource by its identifier (slug, name, CIDR, or vid)
2. If it exists → **PATCH** (update)
3. If it doesn't → **POST** (create)

## Further Reading

- [ARCHITECTURE.md](ARCHITECTURE.md) — Project structure, design decisions, and how to add new resource types
- [vulnerabilities.md](vulnerabilities.md) — Known security considerations
- [NetBox REST API docs](https://netboxlabs.com/docs/netbox/integrations/rest-api/)
- [Bicep extensibility docs](https://github.com/Azure/bicep-extensibility)

## Support

This is a community project maintained on a **best-effort basis**. If you encounter issues or have feature requests:

- Open a [GitHub Issue](../../issues) using the provided templates
- Pull requests are welcome — see [ARCHITECTURE.md](ARCHITECTURE.md) for how to add new resource types
- There are no SLAs or guarantees on response times

## Disclaimer

This project was generated with the assistance of Claude AI (Anthropic) and is provided **as-is**, without warranty of any kind, express or implied. The Bicep extensibility SDK is an experimental feature — there are no guarantees about stability or breaking changes in future Bicep releases. Use at your own risk and always validate in a non-production environment first.

## License

MIT
