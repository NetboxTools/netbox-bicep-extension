# IPSecProfile

An IPSec profile (links IKE policy and IPSec policy).

## API Endpoint

`/api/vpn/ipsec-profiles/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `mode` | string | Yes | IPSec mode: esp, ah. |
| `ikePolicy` | string | Yes | The IKE policy ID. |
| `ipsecPolicy` | string | Yes | The IPSec policy ID. |
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
resource corpProfile 'IPSecProfile' = {
  name: 'Corporate VPN Profile'
  mode: 'esp'
  ikePolicy: ikePolicy.id
  ipsecPolicy: ipsecPolicy.id
  description: 'Standard corporate IPSec profile'
}
```
