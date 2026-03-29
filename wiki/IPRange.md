# IPRange

An arbitrary IP address range.

## API Endpoint

`/api/ipam/ip-ranges/`

## Identifier

`startAddress` - Start IP address in CIDR notation (e.g. '10.0.0.100/24').

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `startAddress` | string | Yes | Start IP address in CIDR notation (e.g. '10.0.0.100/24'). |
| `endAddress` | string | Yes | End IP address in CIDR notation (e.g. '10.0.0.200/24'). |
| `status` | string | No | Operational status: active, reserved, deprecated. |
| `vrf` | string | No | The VRF ID. |
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
resource dhcpPool 'IPRange' = {
  startAddress: '10.0.0.100/24'
  endAddress: '10.0.0.200/24'
  status: 'active'
  vrf: productionVrf.id
  tenant: coreTenant.id
  description: 'DHCP address pool'
}
```
