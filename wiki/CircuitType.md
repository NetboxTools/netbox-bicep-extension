# CircuitType

A type of circuit (e.g. Internet, MPLS, Dark Fiber).

## API Endpoint

`/api/circuits/circuit-types/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `slug` | string | No | URL-friendly identifier (auto-generated from name if omitted, e.g. 'my-datacenter'). |
| `color` | string | No | Color in hex (e.g. 'aa1409'). |
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
resource internetCircuitType 'CircuitType' = {
  name: 'Internet'
  color: '2196f3'
  description: 'Internet transit circuit'
}
```
