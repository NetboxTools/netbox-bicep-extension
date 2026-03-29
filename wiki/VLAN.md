# VLAN

A VLAN.

## API Endpoint

`/api/ipam/vlans/`

## Identifier

`vid` - VLAN ID number (1-4094).

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `vid` | int | Yes | VLAN ID number (1-4094). |
| `name` | string | Yes | VLAN name. |
| `status` | string | No | Operational status: active, reserved, deprecated. |
| `group` | string | No | The VLAN group ID. |
| `site` | string | No | The site ID. |
| `tenant` | string | No | The tenant ID. |
| `role` | string | No | The IPAM role ID. |
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
resource mgmtVlan 'VLAN' = {
  vid: 100
  name: 'Management'
  status: 'active'
  site: stockholmDc.id
  tenant: coreTenant.id
  role: productionRole.id
  description: 'Management VLAN'
}
```
