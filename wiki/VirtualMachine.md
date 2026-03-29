# VirtualMachine

A virtual machine.

## API Endpoint

`/api/virtualization/virtual-machines/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `status` | string | No | Operational status: offline, active, planned, staged, failed, decommissioning, paused. |
| `site` | string | No | The site ID. |
| `cluster` | string | No | The cluster ID. |
| `device` | string | No | The device ID (physical host). |
| `role` | string | No | The device role ID. |
| `tenant` | string | No | The tenant ID. |
| `platform` | string | No | The platform ID (OS/firmware). |
| `serial` | string | No | Serial number. |
| `vcpus` | string | No | Number of virtual CPUs (e.g. '4' or '2.5'). |
| `memory` | string | No | Memory in MB (e.g. '4096'). |
| `disk` | string | No | Disk size in MB (e.g. '102400'). |
| `primaryIp4` | string | No | Primary IPv4 address ID. |
| `primaryIp6` | string | No | Primary IPv6 address ID. |
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
resource webServer01 'VirtualMachine' = {
  name: 'web-server-01'
  status: 'active'
  cluster: prodCluster01.id
  role: webServerRole.id
  tenant: coreTenant.id
  vcpus: '4'
  memory: '8192'
  disk: '102400'
  description: 'Primary web server'
}
```
