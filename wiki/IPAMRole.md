# IPAMRole

An IPAM role for prefixes and VLANs (e.g. Production, Development).

## API Endpoint

`/api/ipam/roles/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `slug` | string | No | URL-friendly identifier (auto-generated from name if omitted, e.g. 'my-datacenter'). |
| `weight` | string | No | Sort weight for ordering. |
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
resource productionRole 'IPAMRole' = {
  name: 'Production'
  weight: '1000'
  description: 'Production network prefixes and VLANs'
}
```
