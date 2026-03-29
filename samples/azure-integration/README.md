# Azure Integration Sample

This sample demonstrates a two-step workflow where an Azure Public IP is deployed first, then registered in NetBox using the Bicep extension.

## Why two files?

Bicep extensions that use `targetScope = 'local'` cannot deploy Azure resources in the same file. Azure deployments require `targetScope = 'resourceGroup'` (or subscription/managementGroup/tenant). These two scopes are incompatible within a single Bicep file, so the workflow is split into two steps.

## Files

| File | Purpose |
|---|---|
| `azure.bicep` | Standard Azure Bicep - creates a static public IP address |
| `netbox.bicep` | Local NetBox extension Bicep - registers the IP in NetBox |
| `netbox.bicepparam` | Parameter file that reads `NETBOX_URL`, `NETBOX_TOKEN`, and `AZURE_PUBLIC_IP` from environment variables |

## Workflow

### Step 1 - Deploy the Azure Public IP

```powershell
$deployment = az deployment group create `
    --resource-group 'rg-netbox-demo' `
    --template-file './azure.bicep' `
    --query 'properties.outputs.ipAddress.value' `
    --output tsv

# Store the IP for step 2 (append /32 for NetBox CIDR notation)
$env:AZURE_PUBLIC_IP = "$deployment/32"
```

### Step 2 - Register the IP in NetBox

```powershell
$env:NETBOX_URL   = 'https://netbox.contoso.com'
$env:NETBOX_TOKEN = '<your-token>'

bicep local-deploy './netbox.bicepparam'
```

### Combined (single script)

```powershell
# Deploy Azure resource and capture the IP
$ip = az deployment group create `
    --resource-group 'rg-netbox-demo' `
    --template-file './azure.bicep' `
    --query 'properties.outputs.ipAddress.value' `
    --output tsv

# Register in NetBox
$env:NETBOX_URL      = 'https://netbox.contoso.com'
$env:NETBOX_TOKEN    = '<your-token>'
$env:AZURE_PUBLIC_IP = "$ip/32"

bicep local-deploy './netbox.bicepparam'
```
