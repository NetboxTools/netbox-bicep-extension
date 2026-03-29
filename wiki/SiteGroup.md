# SiteGroup

A logical group of sites (e.g. corporate, branch, edge).

## API Endpoint

`/api/dcim/site-groups/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `slug` | string | No | URL-friendly identifier (auto-generated from name if omitted, e.g. 'my-datacenter'). |
| `parent` | string | No | The parent group ID (for nesting). |
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
resource corporateSites 'SiteGroup' = {
  name: 'Corporate'
  description: 'Corporate office locations'
}

resource branchSites 'SiteGroup' = {
  name: 'Branch Offices'
  parent: corporateSites.id
  description: 'Regional branch offices'
}
```
