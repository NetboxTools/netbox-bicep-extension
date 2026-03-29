<#
.SYNOPSIS
    Discovers Azure resources via Resource Graph and generates Bicep files for NetBox.

.DESCRIPTION
    Queries Azure Resource Graph for VMs, VNets, Subnets, Public IPs, and Regions,
    then generates a Bicep file that declares the corresponding NetBox resources.
    You can review the generated file and deploy it with 'bicep local-deploy'.

    Requires the Az.ResourceGraph and Az.Accounts PowerShell modules.

.PARAMETER OutputPath
    Path to write the generated Bicep files. Defaults to ./output.

.PARAMETER SubscriptionId
    Azure subscription ID to query. If not specified, uses the current context.

.PARAMETER IncludeVMs
    Include Virtual Machines as NetBox Devices.

.PARAMETER IncludeNetworking
    Include VNets, Subnets, and Public IPs as NetBox Prefixes and IPAddresses.

.PARAMETER IncludeRegions
    Include Azure regions (where resources exist) as NetBox Sites.

.EXAMPLE
    ./Sync-AzureToNetbox.ps1 -IncludeRegions -IncludeNetworking -IncludeVMs

.EXAMPLE
    ./Sync-AzureToNetbox.ps1 -IncludeRegions -OutputPath ./my-output
#>

[CmdletBinding()]
param(
    [string]$OutputPath = "./output",
    [string]$SubscriptionId,
    [switch]$IncludeVMs,
    [switch]$IncludeNetworking,
    [switch]$IncludeRegions
)

$ErrorActionPreference = 'Stop'

# ── Prerequisites ───────────────────────────────────────

function Assert-Module {
    param([string]$Name)
    if (-not (Get-Module -ListAvailable -Name $Name)) {
        throw "Module '$Name' is required. Install it with: Install-Module $Name -Scope CurrentUser"
    }
}

Assert-Module 'Az.Accounts'
Assert-Module 'Az.ResourceGraph'

# Ensure we're connected
$context = Get-AzContext
if (-not $context) {
    throw "Not connected to Azure. Run Connect-AzAccount first."
}

if ($SubscriptionId) {
    Set-AzContext -SubscriptionId $SubscriptionId | Out-Null
}

Write-Host "Querying Azure subscription: $((Get-AzContext).Subscription.Name)" -ForegroundColor Cyan

# ── Query Azure Resource Graph ──────────────────────────

function Invoke-ResourceGraph {
    param([string]$Query)
    $results = Search-AzGraph -Query $Query -First 1000
    return $results
}

$regions = @{}
$bicepResources = [System.Collections.Generic.List[string]]::new()

# ── Regions / Sites ─────────────────────────────────────

if ($IncludeRegions) {
    Write-Host "Discovering Azure regions with resources..." -ForegroundColor Cyan

    $regionQuery = @"
resources
| summarize count() by location
| order by count_ desc
"@

    $regionResults = Invoke-ResourceGraph -Query $regionQuery

    foreach ($r in $regionResults) {
        $location = $r.location
        $slug = $location -replace '[^a-zA-Z0-9]', '-'
        $displayName = ($location -creplace '([A-Z])', ' $1').Trim()
        $displayName = (Get-Culture).TextInfo.ToTitleCase($displayName)

        $regions[$location] = $slug

        $bicepResources.Add(@"

resource site_$($slug -replace '-','_') 'Site' = {
  name: '$displayName'
  slug: '$slug'
  status: 'active'
  description: 'Azure region: $location ($($r.count_) resources)'
}
"@)
    }

    Write-Host "  Found $($regionResults.Count) regions" -ForegroundColor Green
}

# ── Virtual Networks and Subnets ────────────────────────

if ($IncludeNetworking) {
    Write-Host "Discovering Virtual Networks..." -ForegroundColor Cyan

    $vnetQuery = @"
resources
| where type == 'microsoft.network/virtualnetworks'
| project name, location, resourceGroup,
          addressSpace = properties.addressSpace.addressPrefixes,
          subnets = properties.subnets
"@

    $vnets = Invoke-ResourceGraph -Query $vnetQuery

    foreach ($vnet in $vnets) {
        $vnetName = $vnet.name
        $safeName = ($vnetName -replace '[^a-zA-Z0-9]', '_').ToLower()

        # VNet address spaces as Prefixes
        foreach ($prefix in $vnet.addressSpace) {
            $bicepResources.Add(@"

resource prefix_$($safeName) 'Prefix' = {
  prefix: '$prefix'
  status: 'active'
  description: 'Azure VNet: $vnetName ($($vnet.resourceGroup))'
}
"@)
        }

        # Subnets as Prefixes
        foreach ($subnet in $vnet.subnets) {
            $subnetName = $subnet.name
            $safeSubnetName = ($subnetName -replace '[^a-zA-Z0-9]', '_').ToLower()
            $subnetPrefix = $subnet.properties.addressPrefix

            if ($subnetPrefix) {
                $bicepResources.Add(@"

resource prefix_$($safeName)_$($safeSubnetName) 'Prefix' = {
  prefix: '$subnetPrefix'
  status: 'active'
  description: 'Azure Subnet: $vnetName/$subnetName ($($vnet.resourceGroup))'
}
"@)
            }
        }
    }

    Write-Host "  Found $($vnets.Count) VNets" -ForegroundColor Green

    # Public IPs
    Write-Host "Discovering Public IP Addresses..." -ForegroundColor Cyan

    $pipQuery = @"
resources
| where type == 'microsoft.network/publicipaddresses'
| project name, location, resourceGroup,
          ipAddress = properties.ipAddress,
          dnsName = properties.dnsSettings.fqdn
"@

    $pips = Invoke-ResourceGraph -Query $pipQuery

    foreach ($pip in $pips) {
        if (-not $pip.ipAddress) { continue }

        $safeName = ($pip.name -replace '[^a-zA-Z0-9]', '_').ToLower()

        $dnsLine = ""
        if ($pip.dnsName) {
            $dnsLine = "`n  dnsName: '$($pip.dnsName)'"
        }

        $bicepResources.Add(@"

resource ip_$($safeName) 'IPAddress' = {
  address: '$($pip.ipAddress)/32'
  status: 'active'
  description: 'Azure Public IP: $($pip.name) ($($pip.resourceGroup))'$dnsLine
}
"@)
    }

    Write-Host "  Found $($pips.Count) Public IPs" -ForegroundColor Green
}

# ── Virtual Machines ────────────────────────────────────

if ($IncludeVMs) {
    Write-Host "Discovering Virtual Machines..." -ForegroundColor Cyan

    # We need a default Manufacturer, DeviceRole, DeviceType, and Site
    # These are created as prerequisite resources in the Bicep file

    $bicepResources.Insert(0, @"

// ── Azure VM prerequisites ─────────────────────────────
// These are default types for Azure VMs. Customize as needed.

resource manufacturer_microsoft 'Manufacturer' = {
  name: 'Microsoft'
  slug: 'microsoft'
  description: 'Microsoft Azure Virtual Machines'
}

resource role_virtual_machine 'DeviceRole' = {
  name: 'Virtual Machine'
  slug: 'virtual-machine'
  color: '2196f3'
  vmRole: 'true'
  description: 'Azure Virtual Machine'
}
"@)

    $vmQuery = @"
resources
| where type == 'microsoft.compute/virtualmachines'
| project name, location, resourceGroup,
          vmSize = properties.hardwareProfile.vmSize,
          osType = properties.storageProfile.osDisk.osType
"@

    $vms = Invoke-ResourceGraph -Query $vmQuery

    # Collect unique VM sizes to create DeviceTypes
    $vmSizes = $vms | Select-Object -ExpandProperty vmSize -Unique

    foreach ($size in $vmSizes) {
        $safeSize = ($size -replace '[^a-zA-Z0-9]', '_').ToLower()
        $bicepResources.Add(@"

resource devicetype_$($safeSize) 'DeviceType' = {
  manufacturer: 1  // Microsoft — update with actual ID after first deploy
  model: '$size'
  slug: '$($size.ToLower())'
  description: 'Azure VM Size: $size'
}
"@)
    }

    foreach ($vm in $vms) {
        $safeName = ($vm.name -replace '[^a-zA-Z0-9]', '_').ToLower()

        $bicepResources.Add(@"

// VM: $($vm.name) ($($vm.vmSize), $($vm.osType), $($vm.location))
// Note: device_type, role, and site require NetBox IDs.
// Update these after deploying the prerequisite resources above.
// resource device_$($safeName) 'Device' = {
//   name: '$($vm.name)'
//   deviceType: <device_type_id>
//   role: <role_id>
//   site: <site_id>
//   status: 'active'
//   description: 'Azure VM: $($vm.vmSize) / $($vm.osType) ($($vm.resourceGroup))'
// }
"@)
    }

    Write-Host "  Found $($vms.Count) VMs ($($vmSizes.Count) unique sizes)" -ForegroundColor Green
}

# ── Generate Bicep output ───────────────────────────────

if ($bicepResources.Count -eq 0) {
    Write-Host "No resources found. Use -IncludeRegions, -IncludeNetworking, or -IncludeVMs." -ForegroundColor Yellow
    return
}

if (-not (Test-Path $OutputPath)) {
    New-Item -ItemType Directory -Path $OutputPath -Force | Out-Null
}

$bicepContent = @"
// Generated by Sync-AzureToNetbox.ps1 on $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')
// Review this file before deploying. Some resources (Devices) require
// NetBox IDs that must be filled in after creating prerequisite resources.

targetScope = 'local'

@secure()
param netboxToken string
param netboxUrl string

extension netbox with {
  url: netboxUrl
  token: netboxToken
}
$($bicepResources -join "`n")
"@

$bicepFile = Join-Path $OutputPath "netbox-from-azure.bicep"
$bicepContent | Set-Content -Path $bicepFile -Encoding UTF8

# Generate the params file
$paramsContent = @"
using 'netbox-from-azure.bicep'

param netboxUrl = readEnvironmentVariable('NETBOX_URL')
param netboxToken = readEnvironmentVariable('NETBOX_TOKEN')
"@

$paramsFile = Join-Path $OutputPath "netbox-from-azure.bicepparam"
$paramsContent | Set-Content -Path $paramsFile -Encoding UTF8

# Copy bicepconfig.json if it doesn't exist in output
$bicepConfigDest = Join-Path $OutputPath "bicepconfig.json"
if (-not (Test-Path $bicepConfigDest)) {
    $bicepConfigContent = @"
{
  "experimentalFeaturesEnabled": {
    "localDeploy": true,
    "extensibility": true
  },
  "extensions": {
    "netbox": "../extension-publish/bicep-ext-netbox"
  }
}
"@
    $bicepConfigContent | Set-Content -Path $bicepConfigDest -Encoding UTF8
}

Write-Host ""
Write-Host "Generated Bicep files in: $OutputPath" -ForegroundColor Green
Write-Host "  - $bicepFile" -ForegroundColor White
Write-Host "  - $paramsFile" -ForegroundColor White
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Cyan
Write-Host "  1. Review the generated Bicep file" -ForegroundColor White
Write-Host "  2. Update bicepconfig.json extension path if needed" -ForegroundColor White
Write-Host "  3. Set environment variables:" -ForegroundColor White
Write-Host '     $env:NETBOX_URL = "http://localhost:8000"' -ForegroundColor Gray
Write-Host '     $env:NETBOX_TOKEN = "nbt_your-token"' -ForegroundColor Gray
Write-Host "  4. Deploy:" -ForegroundColor White
Write-Host "     bicep local-deploy $paramsFile" -ForegroundColor Gray
