# Prefix

An IP prefix (subnet).

## API Endpoint

`/api/ipam/prefixes/`

## Identifier

`prefix` - IP prefix in CIDR notation (e.g. '10.0.0.0/24').

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `prefix` | string | Yes | IP prefix in CIDR notation (e.g. '10.0.0.0/24'). |
| `status` | string | No | Operational status: container, active, reserved, deprecated. |
| `vrf` | string | No | The VRF ID. |
| `tenant` | string | No | The tenant ID. |
| `vlan` | string | No | The VLAN ID. |
| `role` | string | No | The IPAM role ID. |
| `isPool` | string | No | Treat this prefix as a pool of available addresses (true/false). |
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
resource mgmtPrefix 'Prefix' = {
  prefix: '10.0.0.0/24'
  status: 'active'
  vrf: productionVrf.id
  tenant: coreTenant.id
  vlan: mgmtVlan.id
  role: productionRole.id
  description: 'Management network'
}
```
