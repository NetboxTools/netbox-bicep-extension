# RackType

A rack hardware model (template for racks).

## API Endpoint

`/api/dcim/rack-types/`

## Identifier

`slug` - URL-friendly unique identifier (e.g. 'catalyst-9300').

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `slug` | string | Yes | URL-friendly unique identifier (e.g. 'catalyst-9300'). |
| `manufacturer` | string | Yes | The manufacturer ID. |
| `model` | string | Yes | Rack type model name. |
| `description` | string | No | A brief description. |
| `uHeight` | string | No | Height in rack units (e.g. '42'). |
| `width` | string | No | Rail-to-rail width: 10, 19, 21, 23 (inches). |
| `comments` | string | No | Free-text comments (markdown supported). |

## Output Properties

| Property | Type | Description |
|---|---|---|
| `id` | string | NetBox internal ID (read-only, returned after create/update). |
| `url` | string | API URL for this resource. |
| `display` | string | Display name. |

## Bicep Example

```bicep
resource standardRack 'RackType' = {
  slug: 'apc-netshelter-42u'
  manufacturer: apc.id
  model: 'NetShelter SX 42U'
  uHeight: '42'
  width: '19'
}
```
