# Tunnel

A VPN tunnel.

## API Endpoint

`/api/vpn/tunnels/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `status` | string | Yes | Operational status: planned, active, disabled. |
| `encapsulation` | string | Yes | Encapsulation type: ipsec-transport, ipsec-tunnel, ip-ip, gre. |
| `group` | string | No | The tunnel group ID. |
| `ipsecProfile` | string | No | The IPSec profile ID. |
| `tenant` | string | No | The tenant ID. |
| `tunnelId` | string | No | Numeric tunnel identifier. |
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
resource stkToLon 'Tunnel' = {
  name: 'STK-LON-IPSEC-01'
  status: 'active'
  encapsulation: 'ipsec-tunnel'
  group: siteToSiteTunnels.id
  ipsecProfile: corpProfile.id
  tenant: coreTenant.id
  tunnelId: '100'
  description: 'Stockholm to London IPSec tunnel'
}
```
