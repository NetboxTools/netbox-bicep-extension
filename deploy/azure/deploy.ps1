#!/usr/bin/env pwsh
#Requires -Modules Az.Accounts, Az.Resources
<#
.SYNOPSIS
    Deploy NetBox to Azure (Container Apps + PostgreSQL + Redis).
.DESCRIPTION
    Deploys all resources to rg-netbox in Sweden Central using the Az PowerShell module.
    Prompts for all sensitive parameters on first deploy, saves them to secrets.txt for reuse.
.EXAMPLE
    ./deploy.ps1 -Verbose
    ./deploy.ps1 -ResourceGroup 'rg-netbox-dev' -Location 'westeurope' -Verbose
#>

[CmdletBinding()]
param(
    [string]$ResourceGroup  = 'rg-netbox',
    [string]$Location       = 'swedencentral',
    [string]$SubscriptionId = 'da102798-96df-49fd-8c8c-1e01bf892daf',
    [string]$TenantId       = '773bb731-f752-4a7c-bae1-936a43d7865b'
)

$ErrorActionPreference = 'Stop'
$secretsFile = "$PSScriptRoot/secrets.json"

# ── Ensure logged in to the correct subscription ──
$context = Get-AzContext
if (-not $context -or $context.Subscription.Id -ne $SubscriptionId) {
    Write-Verbose 'Logging in to Azure...'
    Connect-AzAccount -TenantId $TenantId -SubscriptionId $SubscriptionId
}

# ── Create resource group if it doesn't exist ──
if (-not (Get-AzResourceGroup -Name $ResourceGroup -ErrorAction SilentlyContinue)) {
    Write-Verbose "Creating resource group $ResourceGroup in $Location..."
    New-AzResourceGroup -Name $ResourceGroup -Location $Location | Out-Null
}

# ── Load or create secrets ──
if (Test-Path $secretsFile) {
    Write-Verbose "Loading saved secrets from $secretsFile..."
    $secrets = Get-Content $secretsFile -Raw | ConvertFrom-Json

    $postgresPassword = $secrets.postgresPassword | ConvertTo-SecureString -AsPlainText -Force
    $superuserPassword = $secrets.superuserPassword | ConvertTo-SecureString -AsPlainText -Force
    $netboxSecretKey = $secrets.netboxSecretKey | ConvertTo-SecureString -AsPlainText -Force
} else {
    Write-Verbose 'No saved secrets found. Prompting for new values...'

    $pgPwd = Read-Host 'PostgreSQL admin password       '
    $suPwd = Read-Host 'NetBox superuser password       '

    # Generate stable SECRET_KEY (also used as API_TOKEN_PEPPER_1)
    $bytes = [byte[]]::new(50)
    [System.Security.Cryptography.RandomNumberGenerator]::Fill($bytes)
    $skPlain = [Convert]::ToBase64String($bytes)
    Write-Verbose 'Generated Django SECRET_KEY (saved for reuse).'

    # Save to file for subsequent deploys
    @{
        postgresPassword = $pgPwd
        superuserPassword = $suPwd
        netboxSecretKey = $skPlain
    } | ConvertTo-Json | Set-Content $secretsFile
    Write-Verbose "Secrets saved to $secretsFile — keep this file safe."

    $postgresPassword = $pgPwd | ConvertTo-SecureString -AsPlainText -Force
    $superuserPassword = $suPwd | ConvertTo-SecureString -AsPlainText -Force
    $netboxSecretKey = $skPlain | ConvertTo-SecureString -AsPlainText -Force
}

# ── Deploy ──
Write-Verbose "Deploying NetBox to $ResourceGroup ($Location)..."
Write-Verbose 'This takes ~15-20 minutes on first deploy (Redis alone takes ~10 min).'

$result = New-AzResourceGroupDeployment `
    -ResourceGroupName $ResourceGroup `
    -TemplateFile "$PSScriptRoot/main.bicep" `
    -postgresPassword $postgresPassword `
    -netboxSecretKey $netboxSecretKey `
    -superuserPassword $superuserPassword `
    -Verbose

if ($result.ProvisioningState -eq 'Succeeded') {
    $netboxUrl    = $result.Outputs['netboxUrl'].Value
    $postgresHost = $result.Outputs['postgresHost'].Value
    $redisHost    = $result.Outputs['redisHost'].Value

    Write-Output ''
    Write-Output "Deployment succeeded!"
    Write-Output "  NetBox URL : $netboxUrl"
    Write-Output "  PostgreSQL : $postgresHost"
    Write-Output "  Redis      : $redisHost"
    Write-Output ''
    Write-Output 'Set your environment variables:'
    Write-Output "  `$env:NETBOX_URL   = '$netboxUrl'"
    Write-Output "  `$env:NETBOX_TOKEN = '<create a token in the NetBox web UI>'"
} else {
    Write-Error "Deployment failed: $($result.ProvisioningState)"
}
