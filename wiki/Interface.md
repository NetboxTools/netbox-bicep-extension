# Interface

A physical network interface on a device.

## API Endpoint

`/api/dcim/interfaces/`

## Identifier

`name` - Interface name (e.g. 'GigabitEthernet0/0').

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Interface name (e.g. 'GigabitEthernet0/0'). |
| `device` | string | Yes | The device ID. |
| `type` | string | Yes | Interface type (e.g. '1000base-t', '10gbase-x-sfpp'). |
| `enabled` | string | No | Whether the interface is enabled (true/false). |
| `mtu` | string | No | MTU (1-65536). |
| `speed` | string | No | Interface speed in Kbps. |
| `duplex` | string | No | Duplex mode: half, full, auto. |
| `mode` | string | No | 802.1Q mode: access, tagged, tagged-all. |
| `description` | string | No | A brief description. |

## Output Properties

| Property | Type | Description |
|---|---|---|
| `id` | string | NetBox internal ID (read-only, returned after create/update). |
| `url` | string | API URL for this resource. |
| `display` | string | Display name. |

## Bicep Example

```bicep
resource ge00 'Interface' = {
  name: 'GigabitEthernet0/0'
  device: coreSwitch01.id
  type: '1000base-t'
  enabled: 'true'
  mtu: '9000'
  description: 'Uplink to distribution switch'
}
```
