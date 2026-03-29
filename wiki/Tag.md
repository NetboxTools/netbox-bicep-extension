# Tag

A tag for labeling and filtering objects.

## API Endpoint

`/api/extras/tags/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `slug` | string | No | URL-friendly identifier (auto-generated from name if omitted, e.g. 'my-datacenter'). |
| `color` | string | No | Color in hex (e.g. '9e9e9e'). |
| `description` | string | No | A brief description. |

## Output Properties

| Property | Type | Description |
|---|---|---|
| `id` | string | NetBox internal ID (read-only, returned after create/update). |
| `url` | string | API URL for this resource. |
| `display` | string | Display name. |

## Bicep Example

```bicep
resource productionTag 'Tag' = {
  name: 'production'
  color: '4caf50'
  description: 'Production environment resources'
}
```
