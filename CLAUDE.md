# NetBox Bicep Extension — Claude Instructions

## Purpose

Bicep extension that wraps the NetBox REST API, allowing NetBox resources to be declared in Bicep language.

## Background

- Author has PowerShell and Bicep expertise, no C# background — explain C# decisions clearly
- Existing PowerShell module: https://github.com/NetboxTools/NetboxTools
- No permanent NetBox test environment — use Docker locally (see README.md)

## NetBox References

- Product: https://netboxlabs.com/products/netbox/
- REST API docs: https://netboxlabs.com/docs/netbox/integrations/rest-api/
- MCP Integration: https://netboxlabs.com/docs/mcp/
- Swagger (demo): https://demo.netbox.dev/api/schema/swagger-ui/

## Bicep Extension References

- Extensibility SDK: https://github.com/Azure/bicep-extensibility
- Quickstart guide: https://github.com/Azure/bicep/blob/main/docs/experimental/local-deploy-dotnet-quickstart.md
- Sample extensions to follow:
  - https://github.com/anthony-c-martin/bicep-ext-github
  - https://github.com/anthony-c-martin/bicep-ext-keyvault
  - https://github.com/maikvandergaag/bicep-ext-http

---

## Code Rules

### General

- Double-question every code decision — explain reasoning
- No repetitive code — use base classes and shared abstractions
- Follow the structure of the sample Bicep extensions
- Only use types supported by the Bicep SDK: `string`, `string?`, `int`, `bool`, `required` variants — no `int?`, `bool?`, or `double?`

### Security and Compliance

This solution may be deployed in sectors with **high regulatory requirements** (finance, healthcare, government, critical infrastructure). All decisions must account for this:

- **Never commit tokens or secrets** — `token.txt`, `.env` are in `.gitignore`
- Token auth must auto-detect v1 (`Token`) vs v2 (`Bearer`) based on `nbt_` prefix
- Track vulnerabilities and security concerns in `vulnerabilities.md`
- Warn about any new risks you cannot mitigate yourself
- **Third-party packages**: All NuGet dependencies must be documented with name, version, publisher, and purpose. Environments may require packages to be sourced via an internal proxy/mirror — do not assume direct internet access to nuget.org
- **Dependency changes**: Any addition, removal, or version change of a NuGet package must be documented in `vulnerabilities.md` with a risk assessment
- **Compliance impact**: Any feature that handles credentials, user data, or network configuration must document its security implications in `vulnerabilities.md`
- **Minimize dependencies**: Do not add NuGet packages unless absolutely required. Use `HttpClient` and `System.Text.Json` (built into .NET) rather than third-party HTTP or JSON libraries. Current production dependencies: `Azure.Bicep.Local.Extension` and `Bicep.LocalDeploy` only

### Documentation

- Always update `README.md` with useful decisions and information
- Architecture decisions go in `ARCHITECTURE.md`
- Keep sample Bicep files working and tested

### Testing — MANDATORY before committing

**Every commit that changes code or models MUST be validated by:**

1. Build: `dotnet build bicep-ext-netbox.sln`
2. Publish: `dotnet publish src/bicep-ext-netbox.csproj --configuration Release -r win-x64 -o ./extension-publish/win-x64`
3. Package: `bicep publish-extension --bin-win-x64 ./extension-publish/win-x64/bicep-ext-netbox.exe --target ./extension-publish/bicep-ext-netbox --force`
4. Clear Bicep cache: `rm -rf ~/.bicep/local/`
5. Test deploy against NetBox: `bicep local-deploy ./samples/basic/main.bicepparam`
6. If new resource types were added, deploy the relevant sample that exercises them

**Do not commit if any step fails.** NetBox runs locally via Docker in WSL2 on `http://localhost:8000`.

## What-If Support

The `Preview()` handler method is implemented but the Bicep CLI does not expose a `--what-if` flag for local extensions. This is a limitation of the experimental Bicep extensibility SDK. The `Preview()` method is used internally during `bicep build` for type validation only. No create-vs-update diff is available before deploying.

## Adding New Resource Types

1. Add model classes in `src/Models.cs` (identifiers class + resource class with `[ResourceType]` attribute)
2. Create a handler in `src/Handlers/` inheriting from `NetboxResourceHandlerBase<T, TId>` (~10 lines)
3. Register the handler in `src/Program.cs` with `.WithResourceHandler<YourHandler>()`
