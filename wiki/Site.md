# Site

A physical location (datacenter, office, etc.).

## API Endpoint

`/api/dcim/sites/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `slug` | string | No | URL-friendly identifier (auto-generated from name if omitted, e.g. 'my-datacenter'). |
| `status` | string | No | Operational status: planned, staging, active, decommissioning, retired. |
| `region` | string | No | The region ID. |
| `facility` | string | No | Local facility ID or description. |
| `description` | string | No | A brief description of the site. |
| `physicalAddress` | string | No | Physical address of the site. |
| `shippingAddress` | string | No | Shipping address (if different from physical). |
| `timeZone` | string | No | IANA time zone (e.g. 'America/New_York'). |
| `latitude` | string | No | GPS latitude (-90 to 90) as a string (e.g. '59.3293'). |
| `longitude` | string | No | GPS longitude (-180 to 180) as a string (e.g. '18.0686'). |
| `comments` | string | No | Free-text comments (markdown supported). |

## Output Properties

| Property | Type | Description |
|---|---|---|
| `id` | string | NetBox internal ID (read-only, returned after create/update). |
| `url` | string | API URL for this resource. |
| `display` | string | Display name. |

## Bicep Example

```bicep
resource stockholmDc 'Site' = {
  name: 'Stockholm DC1'
  status: 'active'
  region: europeRegion.id
  facility: 'EQX-SK1'
  description: 'Primary datacenter in Stockholm'
  physicalAddress: 'Sveavagen 1, Stockholm, Sweden'
  timeZone: 'Europe/Stockholm'
  latitude: '59.3293'
  longitude: '18.0686'
}
```
