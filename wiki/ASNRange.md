# ASNRange

A range of Autonomous System Numbers.

## API Endpoint

`/api/ipam/asn-ranges/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `slug` | string | No | URL-friendly identifier (auto-generated from name if omitted, e.g. 'my-datacenter'). |
| `rir` | string | Yes | The RIR ID. |
| `start` | int | Yes | Starting ASN. |
| `end` | int | Yes | Ending ASN. |
| `tenant` | string | No | The tenant ID. |
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
resource privateAsnRange 'ASNRange' = {
  name: 'Private ASN Range'
  rir: ripe.id
  start: 64512
  end: 65534
  description: 'RFC 6996 private ASN range'
}
```
