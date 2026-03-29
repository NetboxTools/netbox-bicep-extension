# ClusterType

A cluster technology type (e.g. VMware, Hyper-V, Proxmox).

## API Endpoint

`/api/virtualization/cluster-types/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `slug` | string | No | URL-friendly identifier (auto-generated from name if omitted, e.g. 'my-datacenter'). |
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
resource vmwareType 'ClusterType' = {
  name: 'VMware vSphere'
  description: 'VMware vSphere virtualization platform'
}
```
