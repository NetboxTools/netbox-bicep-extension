# VLANGroup

A VLAN group for organizing VLANs.

## API Endpoint

`/api/ipam/vlan-groups/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `slug` | string | No | URL-friendly identifier (auto-generated from name if omitted, e.g. 'my-datacenter'). |
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
resource dcVlanGroup 'VLANGroup' = {
  name: 'DC VLANs'
  description: 'VLAN group for datacenter networks'
}
```
