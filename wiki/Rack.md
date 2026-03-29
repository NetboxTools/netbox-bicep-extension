# Rack

A physical equipment rack.

## API Endpoint

`/api/dcim/racks/`

## Identifier

`name` - Rack name (unique per site).

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Rack name (unique per site). |
| `site` | string | Yes | The site ID (required). |
| `status` | string | No | Operational status: reserved, available, planned, active, deprecated. |
| `role` | string | No | The rack role ID. |
| `location` | string | No | The location ID (room, floor, etc.). |
| `tenant` | string | No | The tenant ID. |
| `serial` | string | No | Serial number. |
| `assetTag` | string | No | Unique asset tag. |
| `facilityId` | string | No | Facility ID (data center reference). |
| `uHeight` | string | No | Height in rack units (e.g. '42'). |
| `width` | string | No | Rail-to-rail width: 10, 19, 21, 23 (inches). |
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
resource rackA01 'Rack' = {
  name: 'A-01'
  site: stockholmDc.id
  status: 'active'
  role: computeRackRole.id
  location: floor2.id
  uHeight: '42'
  width: '19'
  description: 'Primary compute rack'
}
```
