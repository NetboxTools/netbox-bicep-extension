# Scripts

## Sync-AzureToNetbox.ps1

Discovers your Azure resources via Resource Graph and generates Bicep files for NetBox.

### Prerequisites

```powershell
Install-Module Az.Accounts -Scope CurrentUser
Install-Module Az.ResourceGraph -Scope CurrentUser
Connect-AzAccount
```

### Usage

```powershell
# Discover regions, networking, and VMs
./scripts/Sync-AzureToNetbox.ps1 -IncludeRegions -IncludeNetworking -IncludeVMs

# Just regions and networking
./scripts/Sync-AzureToNetbox.ps1 -IncludeRegions -IncludeNetworking

# Specific subscription
./scripts/Sync-AzureToNetbox.ps1 -SubscriptionId "xxx-xxx" -IncludeRegions

# Custom output path
./scripts/Sync-AzureToNetbox.ps1 -IncludeRegions -OutputPath ./my-output
```

### What it does

| Azure Resource | NetBox Resource | Mapping |
|---|---|---|
| Regions (with resources) | `Site` | Region name as site |
| Virtual Networks | `Prefix` | VNet address space |
| Subnets | `Prefix` | Subnet CIDR |
| Public IPs | `IPAddress` | Allocated IP/32 |
| VM Sizes | `DeviceType` | VM SKU as hardware model |
| Virtual Machines | `Device` | Commented out — needs IDs from first deploy |

### Output

The script generates:
- `netbox-from-azure.bicep` — review and customize before deploying
- `netbox-from-azure.bicepparam` — reads credentials from environment variables
- `bicepconfig.json` — extension reference (update path as needed)

### Two-pass workflow for VMs

Devices in NetBox require foreign key IDs (site, device type, role) that don't exist yet on first run:

1. **First deploy**: Run with `-IncludeRegions` to create Sites, Manufacturers, DeviceTypes, and DeviceRoles
2. **Get IDs**: Look up the created resource IDs in NetBox
3. **Second deploy**: Uncomment the Device resources in the generated Bicep file and fill in the IDs

## publish.ps1

Cross-platform build and package script for the Bicep extension.

```powershell
./scripts/publish.ps1
```
