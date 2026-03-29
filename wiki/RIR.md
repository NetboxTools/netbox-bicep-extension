# RIR

A Regional Internet Registry (e.g. ARIN, RIPE, APNIC).

## API Endpoint

`/api/ipam/rirs/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `slug` | string | No | URL-friendly identifier (auto-generated from name if omitted, e.g. 'my-datacenter'). |
| `isPrivate` | string | No | Whether this is a private registry (true/false). |
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
resource ripe 'RIR' = {
  name: 'RIPE NCC'
  isPrivate: 'false'
  description: 'Reseaux IP Europeens Network Coordination Centre'
}
```
