# VirtualDisk

A virtual disk attached to a virtual machine.

## API Endpoint

`/api/virtualization/virtual-disks/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `virtualMachine` | string | Yes | The virtual machine ID. |
| `size` | int | Yes | Disk size in MB. |
| `description` | string | No | A brief description. |

## Output Properties

| Property | Type | Description |
|---|---|---|
| `id` | string | NetBox internal ID (read-only, returned after create/update). |
| `url` | string | API URL for this resource. |
| `display` | string | Display name. |

## Bicep Example

```bicep
resource osDisk 'VirtualDisk' = {
  name: 'os-disk'
  virtualMachine: webServer01.id
  size: 51200
  description: 'Operating system disk'
}

resource dataDisk 'VirtualDisk' = {
  name: 'data-disk'
  virtualMachine: webServer01.id
  size: 102400
  description: 'Application data disk'
}
```
