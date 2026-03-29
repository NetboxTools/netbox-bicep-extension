# IPSecProposal

An IPSec proposal (transform set).

## API Endpoint

`/api/vpn/ipsec-proposals/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `encryptionAlgorithm` | string | No | Encryption algorithm (e.g. aes-128-cbc, aes-256-cbc, des-cbc). |
| `authenticationAlgorithm` | string | No | Authentication/hash algorithm (e.g. hmac-sha1, hmac-sha256, hmac-md5). |
| `saLifetimeSeconds` | string | No | SA lifetime in seconds. |
| `saLifetimeData` | string | No | SA lifetime in kilobytes of data. |
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
resource ipsecProposal 'IPSecProposal' = {
  name: 'ESP-AES256-SHA256'
  encryptionAlgorithm: 'aes-256-cbc'
  authenticationAlgorithm: 'hmac-sha256'
  saLifetimeSeconds: '3600'
  description: 'Standard IPSec ESP proposal'
}
```
