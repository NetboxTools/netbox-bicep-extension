# ASN

An Autonomous System Number.

## API Endpoint

`/api/ipam/asns/`

## Identifier

`asn` - Autonomous System Number (e.g. 65000).

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `asn` | int | Yes | Autonomous System Number (e.g. 65000). |
| `rir` | string | Yes | The RIR ID (required in NetBox v4.5+). |
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
resource privateAsn 'ASN' = {
  asn: 65001
  rir: ripe.id
  tenant: coreTenant.id
  description: 'Private ASN for Stockholm DC'
}
```
