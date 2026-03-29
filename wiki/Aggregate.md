# Aggregate

A top-level IP aggregate (allocated by a RIR).

## API Endpoint

`/api/ipam/aggregates/`

## Identifier

`prefix` - Aggregate prefix in CIDR notation (e.g. '10.0.0.0/8').

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `prefix` | string | Yes | Aggregate prefix in CIDR notation (e.g. '10.0.0.0/8'). |
| `rir` | string | Yes | The RIR ID. |
| `tenant` | string | No | The tenant ID. |
| `dateAdded` | string | No | Date added (yyyy-MM-dd). |
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
resource rfc1918Class10 'Aggregate' = {
  prefix: '10.0.0.0/8'
  rir: ripe.id
  dateAdded: '2024-01-15'
  description: 'RFC 1918 private address space'
}
```
