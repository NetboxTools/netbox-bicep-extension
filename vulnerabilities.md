# Known Vulnerabilities and Security Considerations

## Active Concerns

### 1. API Token in Plain Text (Medium Risk)
**What:** The NetBox API token is passed as a parameter and stored in environment variables. While marked as `@secure()` in Bicep and `WriteOnly` in the model, the token travels in plain text over HTTP if the NetBox instance is not using HTTPS.

**Mitigation:** Always use HTTPS for your NetBox instance. The extension does not enforce this — it accepts any URL.

**What you can do:** Consider whether we should add HTTPS validation in the `Configuration` class or at least emit a warning for HTTP URLs.

### 2. No TLS Certificate Validation Override (Low Risk — Informational)
**What:** The extension uses the default .NET `HttpClient` which validates TLS certificates. Self-signed certificates (common in lab environments) will cause connection failures.

**What you can do:** If you need self-signed cert support, we'd need to add an optional `ignoreSslErrors` configuration flag. This is a security trade-off worth discussing.

### 3. NuGet Supply Chain (Low Risk)
**What:** The project depends on `Azure.Bicep.Local.Extension` v0.37.4 from NuGet. This is an official Microsoft package, but supply chain attacks on package registries are a known vector.

**Mitigation:** The package is published by Microsoft (Azure organization). Pin exact versions in the `.csproj` to avoid unexpected updates.

### 4. Bicep SDK Type Limitations (Low Risk — Informational)
**What:** The `Azure.Bicep.Local.Extension` SDK (v0.37.4) does not support nullable value types (`int?`, `bool?`, `double?`). Optional numeric and boolean fields must be declared as `string?` and will be serialized as strings to the NetBox API.

**Impact:** NetBox accepts string-encoded numbers/booleans in most cases, but this could cause unexpected behavior if the API strictly validates types in future versions.

**Mitigation:** Required integer fields (like FK IDs on Device) use `required int` which works fine. Only optional fields are affected.

### 5. User Management via IaC (Medium Risk)
**What:** The `User` resource type allows creating and updating NetBox user accounts via Bicep, including setting passwords. Passwords are marked `WriteOnly` so they're never returned, but they are passed as parameters and travel through the deployment pipeline.

**Mitigation:** Always use `@secure()` on password parameters and `readEnvironmentVariable()` in `.bicepparam` files. Never hardcode passwords in Bicep files. Consider whether user management should be done via Bicep at all — the NetBox web UI or LDAP/SAML integration may be more appropriate for production.

## Resolved / Mitigated

_None yet — this is the initial project scaffold._

## Third-Party Dependencies

All NuGet packages used by this project. Environments with regulatory requirements may need to mirror these via an internal proxy.

| Package | Version | Publisher | Purpose | Risk |
|---------|---------|-----------|---------|------|
| `Azure.Bicep.Local.Extension` | 0.37.4 | Microsoft (Azure) | Bicep extensibility SDK — extension host, type system, handler framework | Low — official Microsoft package. Experimental status means breaking changes possible |
| `Bicep.LocalDeploy` | 0.1.3 | Microsoft (Azure) | Enables `targetScope = 'local'` deployments | Low — official Microsoft package |
| `xunit` | 2.9.* | .NET Foundation | Unit testing framework (test project only, not shipped) | None — dev dependency only |
| `xunit.runner.visualstudio` | 2.8.* | .NET Foundation | Test runner (test project only, not shipped) | None — dev dependency only |
| `Microsoft.NET.Test.Sdk` | 17.* | Microsoft | Test infrastructure (test project only, not shipped) | None — dev dependency only |

### Dependency Update Policy

- Pin exact major.minor versions in `.csproj` files
- Any version change must be documented here with a risk assessment
- Test all samples against NetBox after any dependency update

## Notes

- The Bicep extensibility feature is **experimental**. The SDK and protocol may change in future Bicep releases.
- NetBox API tokens have full access by default. Consider using tokens with limited permissions for production use.
- This solution may be deployed in regulated environments. All security concerns and dependency changes must be documented in this file.
