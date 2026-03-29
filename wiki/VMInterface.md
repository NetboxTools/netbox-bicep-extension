# VMInterface

A network interface on a virtual machine.

## API Endpoint

`/api/virtualization/interfaces/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `virtualMachine` | string | Yes | The virtual machine ID. |
| `enabled` | string | No | Whether the interface is enabled (true/false). |
| `mtu` | string | No | MTU (1-65536). |
| `mode` | string | No | 802.1Q mode: access, tagged, tagged-all, q-in-q. |
| `parent` | string | No | The parent interface ID. |
| `bridge` | string | No | The bridge interface ID. |
| `untaggedVlan` | string | No | The untagged VLAN ID. |
| `vrf` | string | No | The VRF ID. |
| `description` | string | No | A brief description. |

## Output Properties

| Property | Type | Description |
|---|---|---|
| `id` | string | NetBox internal ID (read-only, returned after create/update). |
| `url` | string | API URL for this resource. |
| `display` | string | Display name. |

## Bicep Example

```bicep
resource webNic 'VMInterface' = {
  name: 'eth0'
  virtualMachine: webServer01.id
  enabled: 'true'
  mtu: '1500'
  mode: 'access'
  untaggedVlan: mgmtVlan.id
  description: 'Primary network interface'
}
```
