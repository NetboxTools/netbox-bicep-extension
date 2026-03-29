# VRF

A VRF (Virtual Routing and Forwarding) instance.

## API Endpoint

`/api/ipam/vrfs/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `rd` | string | No | Route distinguisher (e.g. '65000:1'). |
| `enforceUnique` | string | No | Enforce unique IP space within this VRF (true/false). |
| `tenant` | string | No | The tenant ID. |
| `description` | string | No | A brief description. |
| `comments` | string | No | Free-text comments (markdown supported). |

## Output Properties

| Property | Type | Description |
|---|---|---|
| `id` | string | NetBox internal ID (read-only, returned after create/update). |
| `url` | string | API URL for this resource. |
| `display` | string | Display name. |

## Bicep Example

```bicep
resource productionVrf 'VRF' = {
  name: 'Production'
  rd: '65000:1'
  enforceUnique: 'true'
  tenant: coreTenant.id
  description: 'Production routing domain'
}
```
