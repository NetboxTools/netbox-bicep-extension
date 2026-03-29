<#
.SYNOPSIS
    Builds and publishes the bicep-ext-netbox extension locally.
.DESCRIPTION
    Cross-compiles for Windows, Linux, and macOS, then packages
    using 'bicep publish-extension' to a local folder.
.PARAMETER OutputPath
    Path to publish the packaged extension. Defaults to ./extension-publish/bicep-ext-netbox.
#>
param(
    [string]$OutputPath = "./extension-publish/bicep-ext-netbox"
)

$ErrorActionPreference = 'Stop'
$projectPath = "$PSScriptRoot/../src"

$rids = @("win-x64", "linux-x64", "osx-arm64")

foreach ($rid in $rids) {
    Write-Host "Building for $rid..." -ForegroundColor Cyan
    dotnet publish $projectPath --configuration Release -r $rid
    if ($LASTEXITCODE -ne 0) { throw "Build failed for $rid" }
}

$basePath = "$projectPath/bin/Release/net9.0"

Write-Host "Packaging extension..." -ForegroundColor Cyan
bicep publish-extension `
    --bin-win-x64 "$basePath/win-x64/publish/bicep-ext-netbox.exe" `
    --bin-linux-x64 "$basePath/linux-x64/publish/bicep-ext-netbox" `
    --bin-osx-arm64 "$basePath/osx-arm64/publish/bicep-ext-netbox" `
    --target $OutputPath `
    --force

Write-Host "Extension published to: $OutputPath" -ForegroundColor Green
