# WirelessLANGroup

A logical group of wireless LANs.

## API Endpoint

`/api/wireless/wireless-lan-groups/`

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
resource corporateWifi 'WirelessLANGroup' = {
  name: 'Corporate Wi-Fi'
  description: 'Corporate wireless network group'
}
```
