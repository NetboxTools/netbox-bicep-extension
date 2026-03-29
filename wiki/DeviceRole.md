# DeviceRole

A functional role for devices (e.g. router, switch, firewall).

## API Endpoint

`/api/dcim/device-roles/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `slug` | string | No | URL-friendly identifier (auto-generated from name if omitted, e.g. 'my-datacenter'). |
| `color` | string | No | Color in hex (e.g. 'aa1409'). |
| `vmRole` | string | No | Whether this role can be assigned to virtual machines (true/false). |
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
resource switchRole 'DeviceRole' = {
  name: 'Access Switch'
  color: '4caf50'
  vmRole: 'false'
  description: 'Layer 2 access switch'
}
```
