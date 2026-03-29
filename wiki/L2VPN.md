# L2VPN

A Layer 2 VPN (e.g. VPLS, VXLAN, EVPN).

## API Endpoint

`/api/vpn/l2vpns/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `slug` | string | No | URL-friendly identifier (auto-generated from name if omitted, e.g. 'my-datacenter'). |
| `type` | string | No | L2VPN type (e.g. vpls, vxlan, vpws, ep-lan, evp-lan, ep-tree). |
| `status` | string | No | Operational status (e.g. active, inactive). |
| `identifier` | string | No | Numeric L2VPN identifier (VNI, VXLAN ID, etc.). |
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
resource vxlanFabric 'L2VPN' = {
  name: 'DC VXLAN Fabric'
  type: 'vxlan'
  status: 'active'
  identifier: '10100'
  tenant: coreTenant.id
  description: 'Datacenter VXLAN overlay fabric'
}
```
