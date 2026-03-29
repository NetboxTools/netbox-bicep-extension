# RouteTarget

A BGP route target.

## API Endpoint

`/api/ipam/route-targets/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
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
resource productionRt 'RouteTarget' = {
  name: '65000:100'
  tenant: coreTenant.id
  description: 'Route target for production VRF'
}
```
