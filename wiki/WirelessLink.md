# WirelessLink

A point-to-point wireless link between two interfaces.

## API Endpoint

`/api/wireless/wireless-links/`

## Identifier

`interfaceA` - The first interface ID (side A).

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `interfaceA` | string | Yes | The first interface ID (side A). |
| `interfaceB` | string | Yes | The second interface ID (side B). |
| `ssid` | string | No | SSID for the wireless link. |
| `status` | string | No | Operational status (e.g. connected, planned, decommissioning). |
| `tenant` | string | No | The tenant ID. |
| `authType` | string | No | Authentication type: open, wep, wpa-personal, wpa-enterprise. |
| `authCipher` | string | No | Authentication cipher: auto, tkip, aes. |
| `authPsk` | string | No | Pre-shared key. |
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
resource buildingLink 'WirelessLink' = {
  interfaceA: apBuildingA.id
  interfaceB: apBuildingB.id
  ssid: 'PTP-LINK-AB'
  status: 'connected'
  tenant: coreTenant.id
  description: 'Point-to-point link between Building A and Building B'
}
```
