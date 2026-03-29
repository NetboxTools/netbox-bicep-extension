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

### Security

- **Never commit tokens or secrets** — `token.txt`, `.env` are in `.gitignore`
- Token auth must auto-detect v1 (`Token`) vs v2 (`Bearer`) based on `nbt_` prefix
- Track vulnerabilities and security concerns in `vulnerabilities.md`
- Warn about any new risks you cannot mitigate yourself

### Documentation

- Always update `README.md` with useful decisions and information
- Architecture decisions go in `ARCHITECTURE.md`
- Keep sample Bicep files working and tested

### Testing

- Build: `dotnet build bicep-ext-netbox.sln`
- Publish: `dotnet publish src/bicep-ext-netbox.csproj --configuration Release -r win-x64 -o ./extension-publish/win-x64`
- Package: `bicep publish-extension --bin-win-x64 ./extension-publish/win-x64/bicep-ext-netbox.exe --target ./extension-publish/bicep-ext-netbox --force`
- Clear Bicep cache after repackaging: `rm -rf ~/.bicep/local/`
- Deploy: `bicep local-deploy ./samples/basic/main.bicepparam`
- NetBox runs locally via Docker in WSL2 on `http://localhost:8000`

## What-If Support

Support for what-if deployments via the `Preview()` handler method. Currently returns desired state without API calls. Planned enhancement: query NetBox during preview to show create-vs-update diff.

## Adding New Resource Types

1. Add model classes in `src/Models.cs` (identifiers class + resource class with `[ResourceType]` attribute)
2. Create a handler in `src/Handlers/` inheriting from `NetboxResourceHandlerBase<T, TId>` (~10 lines)
3. Register the handler in `src/Program.cs` with `.WithResourceHandler<YourHandler>()`
