# Cluster

A virtualization cluster (e.g. a vCenter cluster, AKS cluster).

## API Endpoint

`/api/virtualization/clusters/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `type` | string | Yes | The cluster type ID. |
| `status` | string | No | Operational status: planned, staging, active, decommissioning, offline. |
| `group` | string | No | The cluster group ID. |
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
resource prodCluster01 'Cluster' = {
  name: 'prod-cluster-01'
  type: vmwareType.id
  status: 'active'
  group: productionClusters.id
  tenant: coreTenant.id
  description: 'Primary production VMware cluster'
}
```
