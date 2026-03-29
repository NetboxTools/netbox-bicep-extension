# Location

A location within a site (e.g. building, floor, room, cage).

## API Endpoint

`/api/dcim/locations/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `slug` | string | No | URL-friendly identifier (auto-generated from name if omitted, e.g. 'my-datacenter'). |
| `site` | string | Yes | The site ID (required). |
| `parent` | string | No | The parent location ID (for nesting). |
| `tenant` | string | No | The tenant ID. |
| `status` | string | No | Operational status: planned, staging, active, decommissioning, retired. |
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
resource buildingA 'Location' = {
  name: 'Building A'
  site: stockholmDc.id
  status: 'active'
  description: 'Main server building'
}

resource floor2 'Location' = {
  name: 'Floor 2'
  site: stockholmDc.id
  parent: buildingA.id
  description: 'Second floor server room'
}
```
