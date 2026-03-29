# Host NetBox on Azure

Deploys NetBox to Azure using managed services — no VMs to manage.

## Architecture

| Resource | SKU | Purpose |
|----------|-----|---------|
| Container Apps | 1 vCPU / 2 GiB, scales to zero | NetBox application |
| PostgreSQL Flexible Server | Burstable B1ms | Database |
| Azure Cache for Redis | Basic C0 | Caching and task queue |
| Log Analytics | Free tier (< 5 GiB) | Container logs |

Estimated cost: **~$80/month** in Sweden Central (less with scale-to-zero).

## Prerequisites

- Azure subscription (Visual Studio benefit works)
- PowerShell 7+ with the `Az` module: `Install-Module Az -Scope CurrentUser`

## Deploy

```powershell
cd azure
./deploy.ps1 -Verbose
```

On first run, the script will:
1. Prompt for PostgreSQL admin password and NetBox superuser password
2. Auto-generate a stable `SECRET_KEY` (also used as `API_TOKEN_PEPPER_1`)
3. Save all secrets to `secrets.json` for reuse on subsequent deploys
4. Create the resource group and deploy all resources (~15-20 min)

On subsequent runs, it reloads `secrets.json` automatically — no prompts, same secrets, tokens stay valid.

## After deployment

1. Open the NetBox URL from the deployment output
2. Log in with the superuser credentials you provided
3. Create an API token: **Profile > API Tokens > Add Token**
4. Set your environment variables:

```powershell
$env:NETBOX_URL   = 'https://ca-netbox.<env-id>.swedencentral.azurecontainerapps.io'
$env:NETBOX_TOKEN = '<your token>'
```

## Parameters

| Parameter | Default | Description |
|-----------|---------|-------------|
| `-ResourceGroup` | `rg-netbox` | Azure resource group name |
| `-Location` | `swedencentral` | Azure region |
| `-SubscriptionId` | (hardcoded) | Azure subscription ID |
| `-TenantId` | (hardcoded) | Azure AD tenant ID |

## Scale to zero

The container is configured with `minReplicas: 0`. After ~5 minutes of no traffic, it shuts down and you only pay for database and Redis. First request after idle takes ~30-60 seconds to cold-start.

## Files

| File | Purpose |
|------|---------|
| `main.bicep` | All Azure resources |
| `deploy.ps1` | Deployment script (Az PowerShell) |
| `secrets.json` | Auto-generated, gitignored — stores passwords and SECRET_KEY |

## Security notes

- All secrets are stored as Container App secrets, referenced via `secretRef`
- PostgreSQL requires SSL (`DB_SSLMODE=require`)
- Redis uses TLS on port 6380 (`REDIS_SSL=true`)
- `secrets.json` and `secrets.txt` are gitignored — never commit them
- `CSRF_TRUSTED_ORIGINS` is set to the Container App FQDN
- `LOGIN_REQUIRED=true` — no anonymous access
- `CENSUS_REPORTING_ENABLED=false` — no telemetry
