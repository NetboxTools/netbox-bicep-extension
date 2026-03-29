# RackRole

A functional role for racks (e.g. Compute, Storage, Networking).

## API Endpoint

`/api/dcim/rack-roles/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `slug` | string | No | URL-friendly identifier (auto-generated from name if omitted, e.g. 'my-datacenter'). |
| `color` | string | No | Color in hex (e.g. '4caf50'). |
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
resource computeRackRole 'RackRole' = {
  name: 'Compute'
  color: '4caf50'
  description: 'Racks used for compute servers'
}
```
