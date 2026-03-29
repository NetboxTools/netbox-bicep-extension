# Platform

A device platform (OS/firmware, e.g. Cisco IOS, Junos).

## API Endpoint

`/api/dcim/platforms/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `slug` | string | No | URL-friendly identifier (auto-generated from name if omitted, e.g. 'my-datacenter'). |
| `manufacturer` | string | No | The manufacturer ID. |
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
resource iosXe 'Platform' = {
  name: 'Cisco IOS-XE'
  manufacturer: cisco.id
  description: 'Cisco IOS-XE operating system'
}
```
