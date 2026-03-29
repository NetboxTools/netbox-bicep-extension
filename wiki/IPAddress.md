# IPAddress

An individual IP address.

## API Endpoint

`/api/ipam/ip-addresses/`

## Identifier

`address` - IP address in CIDR notation (e.g. '10.0.0.1/24').

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `address` | string | Yes | IP address in CIDR notation (e.g. '10.0.0.1/24'). |
| `status` | string | No | Operational status: active, reserved, deprecated, dhcp, slaac. |
| `role` | string | No | Functional role: loopback, secondary, anycast, vip, vrrp, hsrp, glbp, carp. |
| `vrf` | string | No | The VRF ID. |
| `tenant` | string | No | The tenant ID. |
| `dnsName` | string | No | DNS hostname. |
| `assignedObjectType` | string | No | Assigned object type (e.g. 'virtualization.vminterface' or 'dcim.interface'). |
| `assignedObjectId` | string | No | Assigned object ID (the interface ID to attach this IP to). |
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
resource switchMgmtIp 'IPAddress' = {
  address: '10.0.0.1/24'
  status: 'active'
  vrf: productionVrf.id
  tenant: coreTenant.id
  dnsName: 'core-sw-01.example.com'
  assignedObjectType: 'dcim.interface'
  assignedObjectId: ge00.id
  description: 'Core switch management IP'
}
```
