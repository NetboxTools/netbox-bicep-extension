# DeviceType

A device hardware model (e.g. Cisco Catalyst 9300, Dell PowerEdge R750).

## API Endpoint

`/api/dcim/device-types/`

## Identifier

`slug` - URL-friendly unique identifier (e.g. 'catalyst-9300').

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `slug` | string | Yes | URL-friendly unique identifier (e.g. 'catalyst-9300'). |
| `manufacturer` | string | Yes | The manufacturer ID. |
| `model` | string | Yes | Model name. |
| `partNumber` | string | No | Part number. |
| `uHeight` | string | No | Height in rack units. |
| `isFullDepth` | string | No | Whether the device takes up the full depth of a rack (true/false). |
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
resource catalyst9300 'DeviceType' = {
  slug: 'catalyst-9300'
  manufacturer: cisco.id
  model: 'Catalyst 9300-48P'
  partNumber: 'C9300-48P-A'
  uHeight: '1'
  isFullDepth: 'true'
}
```
