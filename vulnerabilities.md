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

## Resolved / Mitigated

_None yet — this is the initial project scaffold._

## Notes

- The Bicep extensibility feature is **experimental**. The SDK and protocol may change in future Bicep releases.
- NetBox API tokens have full access by default. Consider using tokens with limited permissions for production use.
