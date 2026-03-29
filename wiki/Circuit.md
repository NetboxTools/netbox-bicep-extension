# Circuit

A communications circuit (e.g. Internet, MPLS, point-to-point).

## API Endpoint

`/api/circuits/circuits/`

## Identifier

`cid` - Circuit ID (unique identifier assigned by the provider).

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `cid` | string | Yes | Circuit ID (unique identifier assigned by the provider). |
| `provider` | string | Yes | The provider ID. |
| `type` | string | Yes | The circuit type ID. |
| `status` | string | No | Operational status: planned, provisioning, active, offline, deprovisioned, decommissioned. |
| `tenant` | string | No | The tenant ID. |
| `installDate` | string | No | Date of installation (yyyy-MM-dd). |
| `terminationDate` | string | No | Date of termination (yyyy-MM-dd). |
| `commitRate` | string | No | Committed rate in Kbps. |
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
resource internetCircuit 'Circuit' = {
  cid: 'TELIA-STK-001'
  provider: telia.id
  type: internetCircuitType.id
  status: 'active'
  tenant: coreTenant.id
  installDate: '2024-03-15'
  commitRate: '1000000'
  description: '1Gbps Internet transit - Stockholm DC'
}
```
