# Device

A physical device (server, switch, router, etc.).

## API Endpoint

`/api/dcim/devices/`

## Identifier

`name` - Device name (unique per site).

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Device name (unique per site). |
| `deviceType` | string | Yes | The device type ID (hardware model). |
| `role` | string | Yes | The device role ID. |
| `site` | string | Yes | The site ID where this device is located. |
| `status` | string | No | Operational status: offline, active, planned, staged, failed, inventory, decommissioning. |
| `rack` | string | No | The rack ID. |
| `position` | string | No | Position in rack (starting from bottom). |
| `face` | string | No | Rack face: front, rear. |
| `tenant` | string | No | The tenant ID. |
| `platform` | string | No | The platform ID (OS/firmware). |
| `serial` | string | No | Serial number. |
| `assetTag` | string | No | Unique asset tag. |
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
resource coreSwitch01 'Device' = {
  name: 'core-sw-01'
  deviceType: catalyst9300.id
  role: switchRole.id
  site: stockholmDc.id
  status: 'active'
  rack: rackA01.id
  position: '20'
  face: 'front'
  platform: iosXe.id
  serial: 'FCW2345G0AB'
}
```
