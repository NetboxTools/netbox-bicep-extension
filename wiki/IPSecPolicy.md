# IPSecPolicy

An IPSec policy (grouping of IPSec proposals).

## API Endpoint

`/api/vpn/ipsec-policies/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `pfsGroup` | string | No | Perfect Forward Secrecy group (e.g. 1, 2, 5, 14, 19, 20). |
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
resource ipsecPolicy 'IPSecPolicy' = {
  name: 'Corporate IPSec Policy'
  pfsGroup: '14'
  description: 'Standard corporate IPSec policy with PFS'
}
```
