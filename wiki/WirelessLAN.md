# WirelessLAN

A wireless LAN (Wi-Fi network).

## API Endpoint

`/api/wireless/wireless-lans/`

## Identifier

`ssid` - Service Set Identifier (SSID).

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `ssid` | string | Yes | Service Set Identifier (SSID). |
| `group` | string | No | The wireless LAN group ID. |
| `status` | string | No | Operational status: active, reserved, disabled, deprecated. |
| `vlan` | string | No | The VLAN ID. |
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
resource corpWifi 'WirelessLAN' = {
  ssid: 'CORP-SECURE'
  group: corporateWifi.id
  status: 'active'
  vlan: wifiVlan.id
  tenant: coreTenant.id
  authType: 'wpa-enterprise'
  authCipher: 'aes'
  description: 'Corporate secure wireless network'
}
```
