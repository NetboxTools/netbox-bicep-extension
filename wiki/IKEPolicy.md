# IKEPolicy

An IKE (Internet Key Exchange) policy.

## API Endpoint

`/api/vpn/ike-policies/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `version` | string | Yes | IKE version: 1, 2. |
| `mode` | string | No | IKE mode: main, aggressive. |
| `presharedKey` | string | No | Pre-shared key. |
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
resource ikePolicy 'IKEPolicy' = {
  name: 'Corporate IKEv2 Policy'
  version: '2'
  mode: 'main'
  description: 'Standard corporate IKEv2 policy'
}
```
